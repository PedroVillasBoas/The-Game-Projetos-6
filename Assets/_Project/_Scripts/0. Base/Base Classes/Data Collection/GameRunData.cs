using System;
using System.Collections.Generic;
using GoodVillageGames.Game.Enums.Enemy;
using GoodVillageGames.Game.Enums.Upgrades;

namespace GoodVillageGames.Game.DataCollection
{
    /// <summary>
    /// Centralized class that hold the data for a player single RUN
    /// </summary>
    [Serializable]
    public class GameRunData
    {
        public DateTime RunStartTime;
        public int RunDifficulty;
        public int TotalRunScore;
        public DateTime RunEndTime;
        public float TotalRunTimeSeconds;
        public float TotalRunTimeMinutes;
        public Dictionary<EnemyType, int> EnemiesDefeated = new();
        public Dictionary<UpgradeRarity, int> UpgradesCollected = new();
        public int NormalShotsFired;
        public int NormalShotsHit;
        public float NormalShotAccuracy;
        public int MissileShotsFired;
        public int MissileShotsHit;
        public float MissileShotAccuracy;
        public Dictionary<string, float> PlayerStats = new();
    }
}