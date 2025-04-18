using System;
using GoodVillageGames.Game.Core.Util.Timer;
using UnityEngine;

namespace GoodVillageGames.Game.Core.Enemy.AI
{
    public abstract class BossAI : EnemyAI
    {
        [SerializeField] private float specialActionCooldownAmount = 20f;
        
        private CountdownTimer specialActionCooldownTimer;

        protected override void Awake()
        {
            base.Awake();

            specialActionCooldownTimer = new(specialActionCooldownAmount);
        }

        public override void Update()
        {
            base.Update();

            specialActionCooldownTimer.Tick(Time.deltaTime);
        }

        protected override void EnemyFire()
        {
            base.EnemyFire();

            if (specialActionCooldownTimer.IsFinished)
            {
                DoSpecialAction();
                specialActionCooldownTimer.Reset();
            }
        }

        protected abstract void DoSpecialAction();
    }
}
