using System;
using UnityEngine;

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

        protected override void Awake()
        {
            base.Awake();

            if (enemyHealthHandler != null)
            {
                enemyHealthHandler.OnDeath += ExecuteDie;
            }
        }

        void OnDestroy()
        {
            if (enemyHealthHandler != null)
            {
                enemyHealthHandler.OnDeath -= ExecuteDie;
            }
        }

        public override void Update()
        {
            base.Update();

            enemyAimHandler.HandleLook(Player.transform.position);

            if (currentState == EnemyState.Die)
                return;

            if (currentState == EnemyState.Chase)
            {
                transform.position = Vector2.MoveTowards(
                    transform.position,
                    Player.position,
                    enemyBaseStats.MaxSpeed * Time.deltaTime
                );
            }

            // Back to Chase
            float distanceToPlayer = Vector2.Distance(transform.position, Player.position);
            if (currentState == EnemyState.DoAction && distanceToPlayer > enemyBaseStats.DoActionRadius)
                currentState = EnemyState.Chase;

            // Fire!
            if (currentState == EnemyState.DoAction)
            {
                Debug.Log("Enemy Shoot!");
                if (!enemyReloadHandler.IsReloading)
                    DoActionEventTriggered?.Invoke(this);
            }
        }

        // Player Area Detection to Fire
        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                currentState = EnemyState.DoAction;
            }
        }

        // Player Left Fire Area
        void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                currentState = EnemyState.Chase;
            }
        }

        void ExecuteDie()
        {
            // Instantiate(deathParticlePrefab, transform.position, Quaternion.identity);
            enemyPooledObject.ReturnToPool();
        }
    }
}
