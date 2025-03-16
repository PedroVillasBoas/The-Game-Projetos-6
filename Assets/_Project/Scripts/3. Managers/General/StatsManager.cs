using UnityEngine;
using GoodVillageGames.Game.General;

namespace GoodVillageGames.Game.Core.Manager
{
    public class StatsManager : MonoBehaviour
    {
        protected Stats _stats;

        public Stats Stats => _stats;
        public int MaxHealth { get; set; }
        public float MaxSpeed { get; set; }

        protected virtual void Awake()
        {
            MaxHealth = Stats.MaxHealth;
            MaxSpeed = Stats.MaxSpeed;
        }
    }
}
