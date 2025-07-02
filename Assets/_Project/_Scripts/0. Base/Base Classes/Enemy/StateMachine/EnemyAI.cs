using System;
using UnityEngine;
using GoodVillageGames.Game.Core.Global;
using GoodVillageGames.Game.Core.Manager;

namespace GoodVillageGames.Game.Core.Enemy.AI
{
    public enum EnemyState
    {
        Chase,
        DoAction,
        Die,
    }

    public class EnemyAI : BaseEnemy
    {
        public EnemyState currentState = EnemyState.Chase;

        public event Action<EnemyAI> DoActionEventTriggered;
        private bool hasBeenActivated = false;


        protected override void OnEnable()
        {
            base.OnEnable();
            currentState = EnemyState.Chase;

            enemyHealthHandler.OnDeath += ExecuteDie;
            enemyDetectionCollider.PlayerInRangeActionTriggered += SetFireFlag;
        }

        private void OnDisable()
        {
            if (hasBeenActivated && MobSpawnAdvisor.Instance != null)
                MobSpawnAdvisor.Instance.DecrementActiveMobCount();
            
            hasBeenActivated = true;
        }

        protected virtual void OnDestroy()
        {
            enemyDetectionCollider.PlayerInRangeActionTriggered -= SetFireFlag;
            enemyHealthHandler.OnDeath -= ExecuteDie;
        }

        public override void Update()
        {
            base.Update();

            // Continuously aim toward the player.
            enemyAimHandler.HandleLook(Player.position);

            if (currentState == EnemyState.Die)
                return;

            ChaseState();
            DoActionState();
        }

        // When in Chase State, go towards the player
        private void ChaseState()
        {
            if (currentState == EnemyState.Chase)
            {
                // Move toward the player only in Chase state.
                transform.position = Vector2.MoveTowards(
                    transform.position,
                    Player.position,
                    enemyBaseStats.MaxSpeed * Time.deltaTime
                );
            }
        }

        // When in DoAction state, fire if off cooldown.
        private void DoActionState()
        {
            if (currentState == EnemyState.DoAction)
            {
                EnemyFire();
            }
        }

        protected virtual void EnemyFire()
        {
            // Only fire when not reloading.
            if (!enemyReloadHandler.IsReloading)
            {
                DoActionEventTriggered?.Invoke(this);
            }
            // If reload is active, stay in DoAction 
                // for now this will stay like this
                // Later I'll do a rotation state
        }

        // This method is called by the EnemyDetectionTrigger
        void SetFireFlag(bool value)
        {
            currentState = value ? EnemyState.DoAction : EnemyState.Chase;
            
            if (!value)
                enemyFireHandler.StopFiring();
        }

        void ExecuteDie()
        {
            Instantiate(enemyDeathVFX, transform.position, Quaternion.identity);
            GlobalEventsManager.Instance.AddDefeatedEnemy(enemyBaseStats.EnemyType);
            ExpSpawnManager.Instance.SpawnEXP(enemyBaseStats.EnemyType, transform.position);
            enemyPooledObject.ReturnToPool();
        }
    }
}
