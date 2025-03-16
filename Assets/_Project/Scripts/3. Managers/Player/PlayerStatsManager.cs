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


        [Title("Player Stats")]
        public float Acceleration  { get; set; }
        
        [Title("Attack")]
        public int AttackDamage  { get; set; }
        public float AttackSpeed  { get; set; }
        public GameObject BulletPrefab  { get; set; }

        [Title("Missile")]
        public int MissileDamage  { get; set; }
        public float MissileCooldown  { get; set; }
        public float MissileAttackSpeed  { get; set; }
        public GameObject MissilePrefab  { get; set; }

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
            Acceleration = PlayerStats.Acceleration;
            AttackDamage = PlayerStats.AttackDamage;
            AttackSpeed = PlayerStats.AttackSpeed;
            BulletPrefab = PlayerStats.BulletPrefab;
            MissileDamage = PlayerStats.MissileDamage;
            MissileCooldown = PlayerStats.MissileCooldown;
            MissileAttackSpeed = PlayerStats.MissileAttackSpeed;
            MissilePrefab = PlayerStats.MissilePrefab;

        }
    }
}
