using System;
using System.IO;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using GoodVillageGames.Game.Enums;
using GoodVillageGames.Game.Enums.Enemy;
using GoodVillageGames.Game.DataCollection;
using GoodVillageGames.Game.Enums.Upgrades;

namespace GoodVillageGames.Game.Core.Global
{
    public class GlobalFileManager : MonoBehaviour
    {
        public static GlobalFileManager Instance { get; private set; }

        public const string FILENAME_SAVEDATA = "-VoidProtocolGameplayData.txt";
        public const string FILE_TXT_HEADER = "SessionID,PlayerName,GameSessionStartTime,GameSessionEndTime,IsFinal,TotalSessionDurationSeconds,TotalSessionDurationMinutes,TotalSessionDurationHours,RunStartTime,RunEndTime,RunDifficulty,TotalRunScore,TotalRunTimeSeconds,TotalRunTimeMinutes,MinionEasyFirst,MinionEasySecond,MinionMediumFirst,MinionMediumSecond,MinionHardFirst,MinionHardSecond,BossEasyFirst,BossEasySecond,BossMediumFirst,BossMediumSecond,BossHardFirst,BossHardSecond,UpgradeCommon,UpgradeUncommon,UpgradeRare,UpgradeEpic,UpgradeLegendary,NormalShotsFired,NormalShotsHit,NormalShotAccuracy,MissileShotsFired,MissileShotsHit,MissileShotAccuracy,TotalPausedCount,QuitedViaPause,StatMaxHealth,StatMaxSpeed,StatMaxDefense,StatBaseAttackDamage,StatAttackSpeed,StatMaxBoostTime,StatMaxBoostSpeed,StatBaseMissileDamage,StatBaseMissileCooldown,StatBoostRechargeRate,StatAcceleration,StatLevel";
        public static string FOLDERNAME_SAVEDATA => Path.Combine(Directory.GetParent(Application.dataPath).FullName, "Players - Gameplay Data");

        // Player Data
        private string playerName = "Anonymous";
        private GameSessionData currentSession;

        // Autosave
        private readonly float saveInterval = 180f;
        private float lastSaveTime;

        // Session Status
        public bool HasActiveSession => currentSession != null;

        // Properties
        public GameSessionData CurrentSession => currentSession;

        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Creating save directory if needed
            try
            {
                if (!Directory.Exists(FOLDERNAME_SAVEDATA))
                    Directory.CreateDirectory(FOLDERNAME_SAVEDATA);
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to create save directory: {e.Message}");
            }
        }

        void Start() => GlobalEventsManager.Instance.ChangeGameStateEventTriggered += OnGameStateChanged;
        void OnDestroy() => GlobalEventsManager.Instance.ChangeGameStateEventTriggered -= OnGameStateChanged;

        void Update()
        {
            if (currentSession == null) return;

            if (Time.time - lastSaveTime > saveInterval)
            {
                SaveDataInFile();
                lastSaveTime = Time.time;
            }
        }

        #region Public Methods

        public void SavePlayerRunStats(Dictionary<string, float> stats) => currentSession.RunData.PlayerStats = stats;

        public void SetPlayerName(string name)
        {
            // Checar o nome digitado
            string newName = string.IsNullOrEmpty(name) ? "Anonymous" : name.Trim();

            // Se não for igual ao nome do playerName atual
            if (!newName.Equals(playerName, StringComparison.OrdinalIgnoreCase))
            {
                playerName = newName;

                // Fechar o arquivo anterior, se tiver algum aberto
                if (currentSession != null)
                    CloseCurrentFile();

                // Checar se o arquivo com o novo nome existe
                string filePath = GetCurrentFilePath();
                CheckFileExist(filePath);
            }
        }

        public void ChangePlayer(string newName)
        {
            if (currentSession != null)
            {
                HandleSessionEnd(GameState.MainMenu);
            }

            string sanitizedName = string.IsNullOrEmpty(newName) ? "Anonymous" : newName.Trim();
            SetPlayerName(sanitizedName);
        }

        public void CloseCurrentFile()
        {
            // Save current session if active
            if (currentSession != null)
            {
                if (currentSession.RunData != null && currentSession.RunData.RunEndTime == DateTime.MinValue)
                    EndCurrentSession(true); // Forcing save with QuitedViaPause = true
                else
                {
                    currentSession.RunData = new();
                    EndCurrentSession(true); // Probrably closed the game or changed the name before playing any run
                }
            }

            // Reseting tracking for new player / new run
            currentSession = null;
        }

        public void StartNewSession()
        {
            currentSession = new GameSessionData
            {
                SessionID = $"{DateTime.Now:yyyyMMddHHmmss}-{Guid.NewGuid():N}"[..20],
                PlayerName = playerName,
                GameSessionStartTime = DateTime.Now,
            };
        }
        #endregion

        #region Private Methods

        void OnApplicationQuit() => CloseCurrentFile();

        void CheckFileExist(string filePath)
        {
            if (!File.Exists(filePath))
                File.WriteAllText(filePath, FILE_TXT_HEADER + Environment.NewLine);

            StartNewSession();
        }

        void OnGameStateChanged(GameState newState)
        {
            if (currentSession == null) return;

            switch (newState)
            {
                case GameState.GameOver:
                case GameState.MainMenu:
                    HandleSessionEnd(newState);
                    break;
            }
        }

        void HandleSessionEnd(GameState endState)
        {
            bool quitViaPause = endState == GameState.MainMenu;
            EndCurrentSession(quitViaPause);
        }

        void EndCurrentSession(bool quitViaPause)
        {
            currentSession.RunData.QuitedViaPause = quitViaPause;

            SaveDataInFile(isFinalSave: true);
            currentSession = null;
        }

        void SaveDataInFile(bool isFinalSave = false)
        {
            if (currentSession == null) return;

            string fileName = $"{playerName}{FILENAME_SAVEDATA}";
            string path = Path.Combine(FOLDERNAME_SAVEDATA, fileName);

            try
            {
                using StreamWriter writer = new(path, true);
                string dataLine = BuildDataLine(isFinalSave);
                writer.WriteLine(dataLine);
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to save data: {e.Message}");
            }
        }

        string BuildDataLine(bool isFinalSave)
        {
            List<string> dataParts = new();
            string[] headers = FILE_TXT_HEADER.Split(',');

            // Generating Session Fingerprint
            string sessionID = currentSession.SessionID;

            // Calculating Current Durations
            float currentSessionSeconds = (float)(DateTime.Now - currentSession.GameSessionStartTime).TotalSeconds;
            float currentSessionMinutes = (float)(DateTime.Now - currentSession.GameSessionStartTime).TotalMinutes;
            float currentSessionHours = (float)(DateTime.Now - currentSession.GameSessionStartTime).TotalHours;

            dataParts.Add(sessionID);
            dataParts.Add(playerName);
            dataParts.Add(currentSession.GameSessionStartTime.ToString("yyyy-MM-ddTHH:mm:ssZ"));
            dataParts.Add(isFinalSave ? currentSession.GameSessionEndTime.ToString("yyyy-MM-ddTHH:mm:ssZ") : "IN_PROGRESS");
            dataParts.Add(isFinalSave.ToString());
            dataParts.Add(currentSessionSeconds.ToString("F0"));
            dataParts.Add(currentSessionMinutes.ToString("F0"));
            dataParts.Add(currentSessionHours.ToString("F0"));
            dataParts.Add(currentSession.RunData.RunStartTime.ToString("yyyy-MM-ddTHH:mm:ssZ"));
            dataParts.Add(currentSession.RunData.RunEndTime.ToString("yyyy-MM-ddTHH:mm:ssZ"));
            dataParts.Add(currentSession.RunData.RunDifficulty.ToString());
            dataParts.Add(currentSession.RunData.TotalRunScore.ToString());
            dataParts.Add(currentSession.RunData.TotalRunTimeSeconds.ToString());
            dataParts.Add(currentSession.RunData.TotalRunTimeMinutes.ToString());

            // Enemies
            foreach (string enemyHeader in headers.Skip(dataParts.Count).TakeWhile(h => h.StartsWith("Minion") || h.StartsWith("Boss")))
            {
                Enum.TryParse(enemyHeader, out EnemyType enemyType);
                int count = currentSession.RunData.EnemiesDefeated.TryGetValue(enemyType, out int val) ? val : 0;
                dataParts.Add(count.ToString());
            }

            // Upgrades
            foreach (string upgradeHeader in headers.Skip(dataParts.Count).TakeWhile(h => h.StartsWith("Upgrade")))
            {
                string rarityName = upgradeHeader.Replace("Upgrade", "");
                Enum.TryParse(rarityName, out UpgradeRarity rarity);
                int count = currentSession.RunData.UpgradesCollected.TryGetValue(rarity, out int val) ? val : 0;
                dataParts.Add(count.ToString());
            }

            dataParts.Add(currentSession.RunData.NormalShotsFired.ToString());
            dataParts.Add(currentSession.RunData.NormalShotsHit.ToString());
            dataParts.Add(currentSession.RunData.NormalShotAccuracy.ToString());
            dataParts.Add(currentSession.RunData.MissileShotsFired.ToString());
            dataParts.Add(currentSession.RunData.MissileShotsHit.ToString());
            dataParts.Add(currentSession.RunData.MissileShotAccuracy.ToString());
            dataParts.Add(currentSession.RunData.TotalPausedCount.ToString());
            dataParts.Add(currentSession.RunData.QuitedViaPause.ToString());

            // Player Stats
            foreach (string statHeader in headers.Skip(dataParts.Count).TakeWhile(h => h.StartsWith("Stat")))
            {
                string statKey = statHeader.Replace("Stat", "");

                if (statKey == "Level" && currentSession.RunData.PlayerStats.TryGetValue(statKey, out float level))
                {
                    dataParts.Add(((int)level).ToString());
                }
                else if (currentSession.RunData.PlayerStats.TryGetValue(statKey, out float value))
                {
                    dataParts.Add(value.ToString("F1"));
                }
                else
                {
                    dataParts.Add("0");
                }
            }

            return string.Join(",", dataParts);

        }

        string GetCurrentFilePath()
        {
            string fileName = $"{playerName}{FILENAME_SAVEDATA}";
            return Path.Combine(FOLDERNAME_SAVEDATA, fileName);
        }
        #endregion
    }
}