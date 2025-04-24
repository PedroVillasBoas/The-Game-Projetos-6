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

        // Properties
        public GameSessionData CurrentSession => currentSession;

        // Data Structure for each gaming session
        [Serializable]
        public class GameSessionData
        {
            public string sessionID;
            public string playerName;
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
            public DateTime runStartTime;
            public DateTime runEndTime;
            public float totalRunTime;
        }

        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
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
            if (currentSession == null) return;

            if (Time.time - lastSaveTime > saveInterval)
            {
                SaveToFile(); // Partial save
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
                sessionID = $"{DateTime.Now:yyyyMMddHHmmss}-{Guid.NewGuid().ToString().Substring(0, 4)}",
                playerName = playerName,
                startTime = DateTime.Now,
                difficulty = (int)GlobalGameManager.Instance.CurrentDifficulty,
                upgradesCollected = new Dictionary<UpgradeRarity, int>(),
                enemiesDefeated = new Dictionary<EnemyType, int>()
            };
        }

        private void OnGameStateChanged(GameState newState)
        {
            if (currentSession == null) return;

            switch (newState)
            {
                case GameState.GameBegin:
                    HandleGameStart();
                    break;

                case GameState.PlayerDied:
                case GameState.GamePaused:
                    CapturePlayerStats();
                    break;

                case GameState.GameOver:
                case GameState.MainMenu:
                    HandleSessionEnd(newState);
                    break;
            }
        }

        private void HandleGameStart() => currentSession.runStartTime = DateTime.Now;

        private void CapturePlayerStats()
        {
            if (PlayerUpgraderManager.Instance != null)
            {
                currentSession.playerStats = PlayerUpgraderManager.Instance.GetPlayerStats();
                currentSession.playerStats["Level"] = PlayerUpgraderManager.Instance.GetPlayerCurrentLevel();
            }

            if (PlayerExpManager.Instance != null)
            {
                currentSession.playerStats["CurrentExp"] = PlayerExpManager.Instance.CurrentExp;
                currentSession.playerStats["ExpToNextLevel"] = PlayerExpManager.Instance.ExpToNextLevel;
            }
        }

        private void HandleSessionEnd(GameState endState)
        {
            currentSession.runEndTime = DateTime.Now;

            if (currentSession.runStartTime != DateTime.MinValue)
            {
                currentSession.totalRunTime =
                    (float)(currentSession.runEndTime - currentSession.runStartTime).TotalSeconds;
            }

            bool quitViaPause = endState == GameState.MainMenu;
            EndCurrentSession(quitViaPause);
        }

        private void EndCurrentSession(bool quitViaPause)
        {
            if (currentSession == null) return;

            currentSession.endTime = DateTime.Now;
            currentSession.quitViaPause = quitViaPause;

            // Run total time
            if (currentSession.runStartTime != DateTime.MinValue && currentSession.runEndTime == DateTime.MinValue)
            {
                currentSession.runEndTime = DateTime.Now;
                currentSession.totalRunTime = (float)(currentSession.runEndTime - currentSession.runStartTime).TotalSeconds;
            }

            CalculateFinalScore();
            SaveToFile(isFinalSave: true);
            sessions.Add(currentSession);
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
            Debug.Log($"Enemy Score: {upgradeScore}");

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
            Debug.Log($"Enemy Score: {enemyScore}");
            // +1 because it's an enum, the first difficulty is 0.
            currentSession.totalScore = (int)((upgradeScore + enemyScore) * (currentSession.difficulty + 1));
            Debug.Log($"Final Score: {currentSession.totalScore}");
        }

        public GameSessionData GetLastSession()
        {
            if (sessions.Count == 0)
                return null;

            return sessions[^1];
        }

        private void SaveToFile(bool isFinalSave = false)
        {
            if (currentSession == null) return;

            string fileName = $"{playerName} - VoidProtocolGameplayData.txt";
            string path = Path.Combine(Application.persistentDataPath, fileName);

#if UNITY_EDITOR
            Debug.Log($"Saving player data to: {path}");
#endif

            try
            {
                bool isNewFile = !File.Exists(path);

                using StreamWriter writer = new(path, true);

                // Write header for new files
                if (isNewFile)
                {
                    writer.WriteLine("SessionID,Player,SessionStart,SessionEnd,DurationSeconds,TotalScore,Difficulty,EnemiesDefeated,UpgradesCollected,NormalAccuracy,MissileAccuracy,QuitViaPause,PlayerStats,IsFinal,TotalRunTime");
                }

                currentSession.normalAccuracy = currentSession.normalShotsFired > 0 ?
                    (float)currentSession.normalShotsHit / currentSession.normalShotsFired : 0f;
                currentSession.missileAccuracy = currentSession.missileShotsFired > 0 ?
                    (float)currentSession.missileShotsHit / currentSession.missileShotsFired : 0f;

                // Generate session fingerprint
                string sessionID = $"{currentSession.startTime:yyyyMMddHHmmss}-{Guid.NewGuid().ToString()[..4]}";

                // Calculate current duration
                float currentDuration = (float)(DateTime.Now - currentSession.startTime).TotalSeconds;

                // Write partial/final record
                writer.WriteLine(
                    $"{sessionID}," +
                    $"{playerName}," +
                    $"{currentSession.startTime:HH:mm:ss dd-MM-yyyy}," +
                    $"{(isFinalSave ? currentSession.endTime.ToString("HH:mm:ss dd-MM-yyyy") : "IN_PROGRESS")}," +
                    $"{currentDuration}," +
                    $"{currentSession.totalScore}," +
                    $"{currentSession.difficulty}," +
                    $"{SerializeDictionary(currentSession.enemiesDefeated)}," +
                    $"{SerializeDictionary(currentSession.upgradesCollected)}," +
                    $"{currentSession.normalAccuracy:P2}," +
                    $"{currentSession.missileAccuracy:P2}," +
                    $"{currentSession.quitViaPause}," +
                    $"{SerializeDictionary(currentSession.playerStats)}," +
                    $"{isFinalSave}," +
                    $"{currentSession.totalRunTime}"
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

            // PersistentDataPath in the Unity Editor for Testing/Debug
#if UNITY_EDITOR
            return Path.Combine(Application.persistentDataPath, fileName);
#else
            // In builds, save to the game's root folder (next to the executable)
            string rootPath = Directory.GetParent(Application.dataPath).FullName;
            return Path.Combine(rootPath, fileName);
#endif
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
            var values = csvLine.Split(',');

            var session = new GameSessionData();
            try
            {
                // 2 → SessionStart
                session.startTime = DateTime.ParseExact(values[2].Trim(),
                    "HH:mm:ss dd-MM-yyyy", CultureInfo.InvariantCulture);

                // 3 → SessionEnd or IN_PROGRESS
                string endStr = values[3].Trim();
                session.endTime = endStr == "IN_PROGRESS"
                    ? DateTime.MinValue
                    : DateTime.ParseExact(endStr, "HH:mm:ss dd-MM-yyyy", CultureInfo.InvariantCulture);

                // 4 → DurationSeconds
                session.durationSeconds = float.Parse(values[4], CultureInfo.InvariantCulture);

                // 5 → TotalScore
                session.totalScore = int.Parse(values[5], CultureInfo.InvariantCulture);

                // 6 → Difficulty
                session.difficulty = int.Parse(values[6], CultureInfo.InvariantCulture);

                // 7 → EnemiesDefeated
                session.enemiesDefeated = ParseEnemyDictionary(values[7]);

                // 8 → UpgradesCollected
                session.upgradesCollected = ParseUpgradeDictionary(values[8]);

                // 9 → NormalAccuracy (e.g. “50.00%”)
                session.normalAccuracy = ParsePercentage(values[9]);

                // 10 → MissileAccuracy
                session.missileAccuracy = ParsePercentage(values[10]);

                // 11 → QuitViaPause (“True”/“False”)
                session.quitViaPause = bool.Parse(values[11]);

                // 12 → PlayerStats
                if (values.Length > 12)
                    session.playerStats = ParsePlayerStats(values[12]);

                if (values.Length > 14)
                    session.totalRunTime = float.Parse(values[14], CultureInfo.InvariantCulture);

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