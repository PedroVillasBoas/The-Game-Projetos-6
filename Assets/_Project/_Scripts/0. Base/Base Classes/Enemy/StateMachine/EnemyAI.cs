using UnityEngine;
using GoodVillageGames.Game.Handlers;

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

        private float actionDistance;
        private float moveSpeed;
        private HealthHandler healthHandler;

        protected override void Awake()
        {
            base.Awake();

            healthHandler = GetComponent<HealthHandler>();
            if (healthHandler != null)
            {
                healthHandler.OnDeath += ExecuteDie;
            }
        }

        private void OnDestroy()
        {
            if (healthHandler != null)
            {
                healthHandler.OnDeath -= ExecuteDie;
            }
        }

        void Start()
        {
            actionDistance = enemyBaseStats.DoActionRadius;
            moveSpeed = enemyBaseStats.MaxSpeed;
        }

        public override void Update()
        {
            base.Update();

            if (currentState == EnemyState.Die)
                return;

            if (currentState == EnemyState.Chase)
            {
                transform.position = Vector2.MoveTowards(
                    transform.position,
                    Player.position,
                    moveSpeed * Time.deltaTime
                );
            }

            float distanceToPlayer = Vector2.Distance(transform.position, Player.position);
            if (currentState == EnemyState.DoAction && distanceToPlayer > actionDistance)
            {
                currentState = EnemyState.Chase;
            }

            if (currentState == EnemyState.DoAction)
            {
                Debug.Log("Enemy Shoot!");

            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                currentState = EnemyState.DoAction;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                currentState = EnemyState.Chase;
            }
        }

        private void ExecuteDie()
        {
            enemyPooledObject.ReturnToPool();
        }
    }
}
