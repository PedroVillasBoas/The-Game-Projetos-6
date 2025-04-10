using System;
using GoodVillageGames.Game.General;
using static GoodVillageGames.Game.Enums.Enums;

namespace GoodVillageGames.Game.Core.Attributes
{
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
                var q = new Query(StatType.MaxHealth, baseStats.MaxSpeed);
                mediator.PerformQuery(this, q);
                return q.Value;
            }
        }
        public float MaxDefense 
        {
            get {
                var q = new Query(StatType.MaxHealth, baseStats.MaxDefense);
                mediator.PerformQuery(this, q);
                return q.Value;
            }
        }
        public float Acceleration 
        {
            get {
                var q = new Query(StatType.MaxHealth, baseStats.Acceleration);
                mediator.PerformQuery(this, q);
                return q.Value;
            }
        }
        public float BaseAttackDamage 
        {
            get {
                var q = new Query(StatType.MaxHealth, baseStats.BaseAttackDamage);
                mediator.PerformQuery(this, q);
                return q.Value;
            }
        }
        public float AttackSpeed 
        {
            get {
                var q = new Query(StatType.MaxHealth, baseStats.AttackSpeed);
                mediator.PerformQuery(this, q);
                return q.Value;
            }
        }
        public float MaxBoostTime 
        {
            get {
                var q = new Query(StatType.MaxHealth, baseStats.MaxBoostTime);
                mediator.PerformQuery(this, q);
                return q.Value;
            }
        }
        public float MaxBoostSpeed 
        {
            get {
                var q = new Query(StatType.MaxHealth, baseStats.MaxBoostSpeed);
                mediator.PerformQuery(this, q);
                return q.Value;
            }
        }
        public float BaseMissileDamage 
        {
            get {
                var q = new Query(StatType.MaxHealth, baseStats.BaseMissileDamage);
                mediator.PerformQuery(this, q);
                return q.Value;
            }
        }
        public float BaseMissileCooldown 
        {
            get {
                var q = new Query(StatType.MaxHealth, baseStats.BaseMissileCooldown);
                mediator.PerformQuery(this, q);
                return q.Value;
            }
        }
        public float MaxMissileAmmo 
        {
            get {
                var q = new Query(StatType.MaxHealth, baseStats.MaxMissileAmmo);
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