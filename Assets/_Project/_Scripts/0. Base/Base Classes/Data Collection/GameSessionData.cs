using System;

namespace GoodVillageGames.Game.DataCollection
{
    /// <summary>
    /// Centralized class that hold the data for a player Game Session
    /// </summary>
    /// <see cref="GameRunData"/>
    [Serializable]
    public class GameSessionData
    {
        public string SessionID;
        public string PlayerName;
        public DateTime GameSessionStartTime;
        public DateTime GameSessionEndTime;
        public float TotalSessionDurationSeconds;
        public float TotalSessionDurationMinutes;
        public float TotalSessionDurationHours;
        public GameRunData RunData;
    }
}