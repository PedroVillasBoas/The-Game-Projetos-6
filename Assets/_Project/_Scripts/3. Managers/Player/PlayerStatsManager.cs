using UnityEngine;
using TriInspector;
using GoodVillageGames.Game.General;
using GoodVillageGames.Game.Interfaces;

namespace GoodVillageGames.Game.Core.Manager
{
    public class PlayerStatsManager : StatsManager, IStatsProvider
    {
        public PlayerStats PlayerStats;

        [Title("Passive")]
        public string PassiveName { get; set; }
        public string PassiveDescription  { get; set; }

        Stats IStatsProvider.Stats => PlayerStats;

        protected override void Awake()
        {
            _stats = PlayerStats;
            base.Awake();

            if (PlayerStats == null)
            {
                Debug.LogError("PlayerStats Scriptable Object was not Assigned!");
                return;
            }

            PassiveName = PlayerStats.PassiveName;
            PassiveDescription = PlayerStats.PassiveDescription;
        }
    }
}
