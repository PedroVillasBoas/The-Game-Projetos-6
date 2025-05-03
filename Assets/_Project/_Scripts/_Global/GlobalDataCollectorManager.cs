using System;
using UnityEngine;
using System.Collections.Generic;
using GoodVillageGames.Game.Enums;
using GoodVillageGames.Game.Enums.Enemy;
using GoodVillageGames.Game.Core.Manager;
using GoodVillageGames.Game.DataCollection;
using GoodVillageGames.Game.Enums.Upgrades;
using GoodVillageGames.Game.Enums.Projectiles;
using GoodVillageGames.Game.Core.Attributes.Modifiers;

namespace GoodVillageGames.Game.Core.Global
{
    public class GlobalDataCollectorManager : MonoBehaviour
    {
        public static GlobalDataCollectorManager Instance { get; private set; }

        private GameRunData currentRunData;

        // Autosave
        private readonly float saveInterval = 180f;
        private float lastSaveTime;


        public GameRunData CurrentRunData => currentRunData;

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
            GlobalEventsManager.Instance.OnGameDifficultyEventTriggered += OnGameDifficulty;
        }

        void OnDestroy()
        {
            GlobalEventsManager.Instance.EnemyDefeatedEventTriggered -= OnEnemyDefeated;
            GlobalEventsManager.Instance.UpgradeCollectedEventTriggered -= OnUpgradeCollected;
            GlobalEventsManager.Instance.ChangeGameStateEventTriggered -= OnGameStateChanged;
            GlobalEventsManager.Instance.OnGameDifficultyEventTriggered -= OnGameDifficulty;
        }

        void Update()
        {
            if (currentRunData == null) return;

            if (Time.time - lastSaveTime > saveInterval)
            {
                AutoSaveRunData();
                lastSaveTime = Time.time;
            }
        }

        void AutoSaveRunData() => SavePlayerCurrentStats();
        void HandleGameStart() => GlobalFileManager.Instance.CurrentSession.RunData = currentRunData;

        void OnGameDifficulty(GameDifficulty difficulty)
        {
            currentRunData = new()
            {
                RunStartTime = DateTime.Now,
                RunDifficulty = (int) GlobalGameManager.Instance.CurrentDifficulty,
            };
        }

        void OnGameStateChanged(GameState newState)
        {
            switch (newState)
            {
                case GameState.Tutorial:
                    HandleGameStart();
                    break;

                case GameState.PlayerDied:
                    SavePlayerCurrentStats();
                    break;

                case GameState.GamePaused:
                    SavePlayerCurrentStats();
                    currentRunData.TotalPausedCount++;
                    break;

                case GameState.GameOver:
                case GameState.MainMenu:
                    HandleRunEnd(newState);
                    break;
            }
        }

        void SavePlayerCurrentStats()
        {
            var upgrader = PlayerUpgraderManager.Instance;
            if (upgrader == null) return;

            var allStats = upgrader.GetPlayerStats();

            var desiredStats = new[]
            {
                "MaxHealth",
                "MaxSpeed",
                "BaseAttackDamage",
                "AttackSpeed",
                "MaxBoostTime",
                "MaxBoostSpeed",
                "BaseMissileDamage",
                "BaseMissileCooldown",
                "BoostRechargeRate",
                "Acceleration",
            };

            var filtered = new Dictionary<string, float>(desiredStats.Length + 1);
            foreach (var key in desiredStats)
            {
                if (allStats.TryGetValue(key, out float value))
                    filtered[key] = value;
            }

            filtered["Level"] = upgrader.GetPlayerCurrentLevel();

            currentRunData.PlayerStats = filtered;
            GlobalFileManager.Instance.SavePlayerRunStats(filtered);
        }

        void HandleRunEnd(GameState endState)
        {
            if (currentRunData == null) return;
            
            currentRunData.RunEndTime = DateTime.Now;

            if (currentRunData.RunStartTime != DateTime.MinValue)
            {
                currentRunData.TotalRunTimeSeconds = (float)(currentRunData.RunEndTime - currentRunData.RunStartTime).TotalSeconds;
                currentRunData.TotalRunTimeMinutes = (float)(currentRunData.RunEndTime - currentRunData.RunStartTime).TotalMinutes;
            }

            EndCurrentRun();
        }

        void EndCurrentRun()
        {
            if (currentRunData == null) return;

            currentRunData.RunEndTime = DateTime.Now;

            // Run total time
            if (currentRunData.RunStartTime != DateTime.MinValue && currentRunData.RunEndTime == DateTime.MinValue)
            {
                currentRunData.RunEndTime = DateTime.Now;
                currentRunData.TotalRunTimeSeconds = (float)(currentRunData.RunEndTime - currentRunData.RunStartTime).TotalSeconds;
                currentRunData.TotalRunTimeMinutes = (float)(currentRunData.RunEndTime - currentRunData.RunStartTime).TotalMinutes;
            }
            
            CalculateAccuracy();
            CalculateFinalScore();
            GlobalFileManager.Instance.SavePlayerRunStats(currentRunData.PlayerStats);
        }

        void CalculateFinalScore()
        {
            double upgradeScore = CalculateUpgradeScore();
            int enemyScore = CalculateEnemyScore();


            currentRunData.TotalRunScore = (int)((upgradeScore + enemyScore) * currentRunData.RunDifficulty);

            Debug.Log($"Enemy Score: {upgradeScore}");
            Debug.Log($"Enemy Score: {enemyScore}");
            Debug.Log($"Final Score: {currentRunData.TotalRunScore}");
        }

        double CalculateUpgradeScore()
        {
            double upgradeScore = 0;
            foreach (var upgrade in currentRunData.UpgradesCollected)
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

            return upgradeScore;
        }

        int CalculateEnemyScore()
        {
            int enemyScore = 0;
            foreach (var enemy in currentRunData.EnemiesDefeated)
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

            return enemyScore;
        }

        void CalculateAccuracy()
        {
            currentRunData.NormalShotAccuracy = currentRunData.NormalShotsFired > 0 ? (float)currentRunData.NormalShotsHit / currentRunData.NormalShotsFired : 0f;
            currentRunData.MissileShotAccuracy = currentRunData.MissileShotsFired > 0 ? (float)currentRunData.MissileShotsHit / currentRunData.MissileShotsFired : 0f;
        }

        // Event Handlers
        void OnEnemyDefeated(EnemyType type)
        {
            if (currentRunData.EnemiesDefeated.ContainsKey(type))
                currentRunData.EnemiesDefeated[type]++;
            else
                currentRunData.EnemiesDefeated.Add(type, 1);
        }

        void OnUpgradeCollected(UpgradeStatModifier upgrade)
        {
            var rarity = upgrade.Rarity;
            if (currentRunData.UpgradesCollected.ContainsKey(rarity))
                currentRunData.UpgradesCollected[rarity]++;
            else
                currentRunData.UpgradesCollected.Add(rarity, 1);
        }

        public void RegisterShot(ProjectileType type)
        {
            if (currentRunData == null) return;

            switch (type)
            {
                case ProjectileType.Basic:
                    currentRunData.NormalShotsFired++;
                    break;
                case ProjectileType.Missile:
                    currentRunData.MissileShotsFired++;
                    break;
            }
        }

        public void RegisterHit(ProjectileType type)
        {
            if (currentRunData == null) return;

            switch (type)
            {
                case ProjectileType.Basic:
                    currentRunData.NormalShotsHit++;
                    break;
                case ProjectileType.Missile:
                    currentRunData.MissileShotsHit++;
                    break;
            }
        }
    }
}