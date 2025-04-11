using System;
using GoodVillageGames.Game.General;
using static GoodVillageGames.Game.Enums.Enums;

namespace GoodVillageGames.Game.Core.Attributes
{
    /// <summary>
    /// When a Stat is GET, the mediator will calculate the total value based on modifiers that the Stat can have and return the Result;
    /// </summary>
    /// <remarks>
    /// <param name="BaseStats"> Scriptable Object that contains the starting stats for the entity;
    /// <param name="StatsMediator"> Mediator / Broker  
    /// </remarks>
    public class Stats 
    {
        readonly StatsMediator mediator;
        readonly BaseStats baseStats;

        public StatsMediator Mediator => mediator;

        public float MaxHealth 
        {
            get {
                var q = new Query(StatType.MaxHealth, baseStats.MaxHealth);
                mediator.PerformQuery(this, q);
                return q.Value;
            }
        }
        public float MaxSpeed 
        {
            get {
                var q = new Query(StatType.MaxSpeed, baseStats.MaxSpeed);
                mediator.PerformQuery(this, q);
                return q.Value;
            }
        }
        public float MaxDefense 
        {
            get {
                var q = new Query(StatType.MaxDefense, baseStats.MaxDefense);
                mediator.PerformQuery(this, q);
                return q.Value;
            }
        }
        public float Acceleration 
        {
            get {
                var q = new Query(StatType.Acceleration, baseStats.Acceleration);
                mediator.PerformQuery(this, q);
                return q.Value;
            }
        }
        public float BaseAttackDamage 
        {
            get {
                var q = new Query(StatType.BaseAttackDamage, baseStats.BaseAttackDamage);
                mediator.PerformQuery(this, q);
                return q.Value;
            }
        }
        public float AttackSpeed 
        {
            get {
                var q = new Query(StatType.AttackSpeed, baseStats.AttackSpeed);
                mediator.PerformQuery(this, q);
                return q.Value;
            }
        }
        public float MaxBoostTime 
        {
            get {
                var q = new Query(StatType.MaxBoostTime, baseStats.MaxBoostTime);
                mediator.PerformQuery(this, q);
                return q.Value;
            }
        }
        public float MaxBoostSpeed 
        {
            get {
                var q = new Query(StatType.MaxBoostSpeed, baseStats.MaxBoostSpeed);
                mediator.PerformQuery(this, q);
                return q.Value;
            }
        }
        public float BaseMissileDamage 
        {
            get {
                var q = new Query(StatType.BaseMissileDamage, baseStats.BaseMissileDamage);
                mediator.PerformQuery(this, q);
                return q.Value;
            }
        }
        public float BaseMissileCooldown 
        {
            get {
                var q = new Query(StatType.BaseMissileCooldown, baseStats.BaseMissileCooldown);
                mediator.PerformQuery(this, q);
                return q.Value;
            }
        }
        public float MaxMissileAmmo 
        {
            get {
                var q = new Query(StatType.MaxMissileAmmo, baseStats.MaxMissileAmmo);
                mediator.PerformQuery(this, q);
                return q.Value;
            }
        }
        public float Experience 
        {
            get {
                var q = new Query(StatType.Experience, baseStats.Experience);
                mediator.PerformQuery(this, q);
                return q.Value;
            }
        }
        public Stats(StatsMediator mediator, BaseStats baseStats)
        {
            this.mediator = mediator;
            this.baseStats = baseStats;
        }
    }
}