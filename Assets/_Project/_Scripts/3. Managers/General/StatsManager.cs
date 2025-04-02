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
        public int AttackDamage { get; set; }
        public float AttackSpeed { get; set; }
        public GameObject BulletPrefab { get; set; }
        public float Acceleration { get; set; }


        protected virtual void Awake()
        {
            MaxHealth = Stats.MaxHealth;
            MaxSpeed = Stats.MaxSpeed;
            AttackDamage = Stats.AttackDamage;
            AttackSpeed = Stats.AttackSpeed;
            BulletPrefab = Stats.BulletPrefab;
            Acceleration = Stats.Acceleration;
        }
    }
}
