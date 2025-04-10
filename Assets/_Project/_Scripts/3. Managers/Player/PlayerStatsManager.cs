using UnityEngine;
using TriInspector;
using GoodVillageGames.Game.General;
using GoodVillageGames.Game.Interfaces;

namespace GoodVillageGames.Game.Core.Manager
{
    public class PlayerStatsManager : StatsManager, IStatsProvider
    {
        public PlayerStats PlayerStats;

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
        }
    }
}
