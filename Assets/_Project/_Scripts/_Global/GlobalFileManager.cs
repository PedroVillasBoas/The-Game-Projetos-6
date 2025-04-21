using System;
using System.IO;
using UnityEngine;
using System.Globalization;
using System.Collections.Generic;
using GoodVillageGames.Game.Core.Manager;
using static GoodVillageGames.Game.Enums.Enums;
using GoodVillageGames.Game.Core.Attributes.Modifiers;

namespace GoodVillageGames.Game.Core.Global
{
    public class GlobalFileManager : MonoBehaviour
    {
        public static GlobalFileManager Instance { get; private set; }

        // Player Data
        private string playerName = "Anonymous";
        private List<GameSessionData> sessions = new();
        private GameSessionData currentSession;

        // Autosave
        private readonly float saveInterval = 180f;
        private float lastSaveTime;

        // Session Status
        public bool HasActiveSession => currentSession != null;

        // Data Structure for each gaming session
        [Serializable]
        private class GameSessionData
        {
            public DateTime startTime;
            public DateTime endTime;
            public float durationSeconds;
            public int difficulty;
            public int totalScore;
            public Dictionary<EnemyType, int> enemiesDefeated = new();
            public Dictionary<UpgradeRarity, int> upgradesCollected = new();
            public int normalShotsFired;
            public int normalShotsHit;
            public float normalAccuracy;
            public int missileShotsFired;
            public int missileShotsHit;
            public float missileAccuracy;
            public bool quitViaPause;
            public Dictionary<string, float> playerStats = new();
        }

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
                Destroy(gameObject);
        }

        void Start()
        {
            GlobalEventsManager.Instance.EnemyDefeatedEventTriggered += OnEnemyDefeated;
            GlobalEventsManager.Instance.UpgradeCollectedEventTriggered += OnUpgradeCollected;
            GlobalEventsManager.Instance.ChangeGameStateEventTriggered += OnGameStateChanged;
        }

        void OnDestroy()
        {
            GlobalEventsManager.Instance.EnemyDefeatedEventTriggered -= OnEnemyDefeated;
            GlobalEventsManager.Instance.UpgradeCollectedEventTriggered -= OnUpgradeCollected;
            GlobalEventsManager.Instance.ChangeGameStateEventTriggered -= OnGameStateChanged;
        }

        void Update()
        {
            if (Time.time - lastSaveTime > saveInterval)
            {
                SaveToFile();
                lastSaveTime = Time.time;
            }
        }

        public void SetPlayerName(string name)
        {
            string newName = string.IsNullOrEmpty(name) ? "Anonymous" : name.Trim();

            // Only reset IF name actually changes
            if (!newName.Equals(playerName, StringComparison.OrdinalIgnoreCase))
            {
                playerName = newName;
                sessions.Clear();
            }

            // Loading existing sessions IF file exists
            string filePath = GetCurrentFilePath();
            if (File.Exists(filePath))
            {
                LoadExistingSessions(filePath);
            }

            StartNewSession();
        }

        private void LoadExistingSessions(string filePath)
        {
            sessions.Clear();

            try
            {
                using StreamReader reader = new(filePath);
                reader.ReadLine(); // Skip header

                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();

                    // Parse line into GameSessionData and add to sessions
                    GameSessionData session = ParseSession(line);
                    sessions.Add(session);
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Error loading sessions: {e.Message}");
            }
        }

        public void StartNewSession()
        {
            currentSession = new GameSessionData
            {
                startTime = DateTime.Now,
                difficulty = (int)GlobalGameManager.Instance.CurrentDifficulty,
                upgradesCollected = new Dictionary<UpgradeRarity, int>(),
                enemiesDefeated = new Dictionary<EnemyType, int>()
            };
        }

        private void OnGameStateChanged(GameState newState)
        {
            // Capturing Player Stats WHENEVER gameplay is suspended
            if (newState == GameState.PlayerDied || newState == GameState.GamePaused)
            {
                if (PlayerUpgraderManager.Instance != null && PlayerExpManager.Instance != null)
                {
                    currentSession.playerStats = PlayerUpgraderManager.Instance.GetPlayerStats();
                    currentSession.playerStats["Level"] = PlayerUpgraderManager.Instance.GetPlayerCurrentLevel();
                    currentSession.playerStats["CurrentExp"] = PlayerExpManager.Instance.CurrentExp;
                    currentSession.playerStats["ExpToNextLevel"] = PlayerExpManager.Instance.ExpToNextLevel;
                }
            }

            // Finalizing current session when reaching terminal states
            if (newState == GameState.GameOver || newState == GameState.MainMenu)
            {
                bool quitViaPause = newState == GameState.MainMenu;
                EndCurrentSession(quitViaPause);
            }
        }

        private void EndCurrentSession(bool quitViaPause)
        {
            if (currentSession == null) return;

            currentSession.endTime = DateTime.Now;
            currentSession.quitViaPause = quitViaPause;
            CalculateFinalScore();
            sessions.Add(currentSession);
            SaveToFile();

            // Clearing current session AFTER saving
            currentSession = null;
        }

        private void CalculateFinalScore()
        {
            double upgradeScore = 0;
            foreach (var upgrade in currentSession.upgradesCollected)
            {
                upgradeScore += upgrade.Key switch
                {
                    UpgradeRarity.Common => upgrade.Value * 1,
                    UpgradeRarity.Uncommon => upgrade.Value * 1.5,
                    UpgradeRarity.Rare => upgrade.Value * 2,
                    UpgradeRarity.Epic => upgrade.Value * 3,
                    UpgradeRarity.Legendary => upgrade.Value * 5,
                    _ => 0
                };
            }

            int enemyScore = 0;
            foreach (var enemy in currentSession.enemiesDefeated)
            {
                enemyScore += enemy.Key switch
                {
                    EnemyType.MinionEasyFirst => enemy.Value * 10,
                    EnemyType.MinionMediumFirst => enemy.Value * 12,
                    EnemyType.MinionHardFirst => enemy.Value * 14,
                    EnemyType.MinionEasySecond => enemy.Value * 12,
                    EnemyType.MinionMediumSecond => enemy.Value * 13,
                    EnemyType.MinionHardSecond => enemy.Value * 15,
                    EnemyType.BossEasyFirst => enemy.Value * 20,
                    EnemyType.BossMediumFirst => enemy.Value * 25,
                    EnemyType.BossMediumSecond => enemy.Value * 30,
                    EnemyType.BossHardFirst => enemy.Value * 40,
                    EnemyType.BossHardSecond => enemy.Value * 50,
                    _ => 0
                };
            }

            currentSession.totalScore = (int)((upgradeScore + enemyScore) * currentSession.difficulty);
        }

        private void SaveToFile()
        {
            string fileName = $"{playerName} - VoidProtocolGameplayData.txt";
            string path = Path.Combine(Application.persistentDataPath, fileName);

#if UNITY_EDITOR
            Debug.Log($"Saving player data to: {path}");
#endif

            try
            {
                using StreamWriter writer = new(path, true);

                // Calculating and storing session metrics
                TimeSpan duration = currentSession.endTime - currentSession.startTime;
                currentSession.durationSeconds = (float)duration.TotalSeconds;

                currentSession.normalAccuracy = currentSession.normalShotsFired > 0 ?
                    (float)currentSession.normalShotsHit / currentSession.normalShotsFired : 0f;

                currentSession.missileAccuracy = currentSession.missileShotsFired > 0 ?
                    (float)currentSession.missileShotsHit / currentSession.missileShotsFired : 0f;

                // Write header IF it's a new file
                if (new FileInfo(path).Length == 0)
                {
                    writer.WriteLine("SessionStart,SessionEnd,DurationSeconds,TotalScore,Difficulty,EnemiesDefeated,UpgradesCollected,NormalAccuracy,MissileAccuracy,QuitViaPause,PlayerStats");
                }

                // Session data
                writer.WriteLine(
                    $"{currentSession.startTime:HH:mm:ss dd-MM-yyyy}," +
                    $"{currentSession.endTime:HH:mm:ss dd-MM-yyyy}," +
                    $"{currentSession.durationSeconds}," + // Use stored value
                    $"{currentSession.totalScore}," +
                    $"{currentSession.difficulty}," +
                    $"{SerializeDictionary(currentSession.enemiesDefeated)}," +
                    $"{SerializeDictionary(currentSession.upgradesCollected)}," +
                    $"{currentSession.normalAccuracy:P0}," + // Use stored value
                    $"{currentSession.missileAccuracy:P0}," + // Use stored value
                    $"{currentSession.quitViaPause}," +
                    $"{SerializeDictionary(currentSession.playerStats)}" // Use stored value
                );
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to save data: {e.Message}");
            }
        }

        private string SerializeDictionary<TKey, TValue>(Dictionary<TKey, TValue> dict)
        {
            List<string> entries = new();
            foreach (var pair in dict)
            {
                entries.Add($"{pair.Key}:{pair.Value}");
            }
            return string.Join("|", entries);
        }

        public void CloseCurrentFile()
        {
            // Save current session if active
            if (currentSession != null && currentSession.endTime == DateTime.MinValue)
            {
                EndCurrentSession(true); // Force save with quitViaPause = true
            }

            // Reseting tracking for new player
            sessions.Clear();
            currentSession = null;
        }

        public void ChangePlayer(string newName)
        {
            if (currentSession != null)
            {
                Debug.LogWarning("Active session exists! Finish game first.");
                return;
            }

            string sanitizedName = string.IsNullOrEmpty(newName) ? "Anonymous" : newName.Trim();
            SetPlayerName(sanitizedName);
        }

        private string GetCurrentFilePath()
        {
            string fileName = $"{playerName} - VoidProtocolGameplayData.txt";
            return Path.Combine(Application.persistentDataPath, fileName);
        }

        // Event Handlers
        private void OnEnemyDefeated(EnemyType type)
        {
            if (currentSession.enemiesDefeated.ContainsKey(type))
                currentSession.enemiesDefeated[type]++;
            else
                currentSession.enemiesDefeated.Add(type, 1);
        }

        private void OnUpgradeCollected(UpgradeStatModifier upgrade)
        {
            var rarity = upgrade.Rarity;
            if (currentSession.upgradesCollected.ContainsKey(rarity))
                currentSession.upgradesCollected[rarity]++;
            else
                currentSession.upgradesCollected.Add(rarity, 1);
        }

        public void RegisterShot(ProjectileType type)
        {
            if (currentSession == null) return;

            switch (type)
            {
                case ProjectileType.Basic:
                    currentSession.normalShotsFired++;
                    break;
                case ProjectileType.Missile:
                    currentSession.missileShotsFired++;
                    break;
            }
        }

        public void RegisterHit(ProjectileType type)
        {
            if (currentSession == null) return;

            switch (type)
            {
                case ProjectileType.Basic:
                    currentSession.normalShotsHit++;
                    break;
                case ProjectileType.Missile:
                    currentSession.missileShotsHit++;
                    break;
            }
        }

        void OnApplicationQuit()
        {
            CloseCurrentFile();
        }

#if UNITY_EDITOR // Also temp -> Remove Later
        [ContextMenu("Open Save Folder")]
        void OpenSaveFolder()
        {
            Application.OpenURL(Application.persistentDataPath);
        }
#endif

        private GameSessionData ParseSession(string csvLine)
        {
            GameSessionData session = new();
            string[] values = csvLine.Split(',');

            try
            {
                // Parse dates
                session.startTime = DateTime.ParseExact(values[0].Trim(),
                    "HH:mm:ss dd-MM-yyyy", CultureInfo.InvariantCulture);

                session.endTime = DateTime.ParseExact(values[1].Trim(),
                    "HH:mm:ss dd-MM-yyyy", CultureInfo.InvariantCulture);

                // Parse numerical values
                session.durationSeconds = (int)float.Parse(values[2]);
                session.totalScore = int.Parse(values[3]);
                session.difficulty = int.Parse(values[4]);

                // Parse dictionaries
                session.enemiesDefeated = ParseEnemyDictionary(values[5]);
                session.upgradesCollected = ParseUpgradeDictionary(values[6]);

                // Parse accuracies
                session.normalAccuracy = ParsePercentage(values[7]);
                session.missileAccuracy = ParsePercentage(values[8]);

                // Parse boolean
                session.quitViaPause = bool.Parse(values[9]);

                if (values.Length > 10)
                    session.playerStats = ParsePlayerStats(values[10]);
    
                return session;
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to parse session: {e.Message}");
                return null;
            }
        }

        private Dictionary<EnemyType, int> ParseEnemyDictionary(string input)
        {
            var dict = new Dictionary<EnemyType, int>();
            string[] entries = input.Split('|');

            foreach (string entry in entries)
            {
                string[] parts = entry.Split(':');
                if (parts.Length == 2 && Enum.TryParse(parts[0], out EnemyType type))
                    dict[type] = int.Parse(parts[1]);
            }
            return dict;
        }

        private Dictionary<UpgradeRarity, int> ParseUpgradeDictionary(string input)
        {
            var dict = new Dictionary<UpgradeRarity, int>();
            string[] entries = input.Split('|');

            foreach (string entry in entries)
            {
                string[] parts = entry.Split(':');
                if (parts.Length == 2 && Enum.TryParse(parts[0], out UpgradeRarity rarity))
                    dict[rarity] = int.Parse(parts[1]);
            }
            return dict;
        }

        private float ParsePercentage(string input)
        {
            if (string.IsNullOrEmpty(input)) return 0;
            string clean = input.Trim().Replace("%", "");
            return float.TryParse(clean, NumberStyles.Any, CultureInfo.InvariantCulture, out float result) ? result / 100f : 0f;
        }

        private Dictionary<string, float> ParsePlayerStats(string input)
        {
            var dict = new Dictionary<string, float>();
            string[] entries = input.Split('|');

            foreach (string entry in entries)
            {
                string[] parts = entry.Split(':');
                if (parts.Length == 2 && float.TryParse(parts[1], NumberStyles.Any, CultureInfo.InvariantCulture, out float value))
                {
                    dict[parts[0]] = value;
                }
            }
            return dict;
        }
    }
}