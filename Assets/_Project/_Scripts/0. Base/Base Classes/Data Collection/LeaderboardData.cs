using System;

namespace GoodVillageGames.Game.DataCollection
{
    /// <summary>
    /// Centralized class that hold the data for a player single RUN
    /// </summary>
    /// <see cref="GameRunData"/>
    [Serializable]
    public class LeaderboardData
    {
        public string SessionID;
        public float TotalRunScore;
        public float TotalRunTime;
        public int TotalEnemiesDefeated;
        public int UpgradesCollected;
        public float NormalShotAccuracy;
        public float MissileShotAccuracy;
    }
}