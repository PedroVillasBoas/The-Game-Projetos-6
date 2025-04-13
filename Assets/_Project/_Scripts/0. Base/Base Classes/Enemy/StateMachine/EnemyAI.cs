using UnityEngine;
using UnityEngine.AI;
using GoodVillageGames.Game.General;
using GoodVillageGames.Game.Core.Pooling;
using KBCore.Refs;

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

        //[Child] private FireHandler fireHandler;
        //[Child] private ReloadHandler reloadHandler;
        private float actionDistance;

        void Start()
        {
            actionDistance = enemyBaseStats.DoActionRadius;
        }

        public override void Update()
        {
            base.Update();

            if (currentState == EnemyState.Die)
                return;

            float distanceToPlayer = Vector3.Distance(transform.position, Player.position);

            switch (currentState)
            {
                case EnemyState.Chase:
                    // Move to the position of the player
                    if (distanceToPlayer <= actionDistance)
                        currentState = EnemyState.DoAction;
                        
                    break;

                case EnemyState.DoAction:
                    Debug.Log("Enemy Shoot!");
                    // if (!reloadHandler.IsReloading)
                    //     fireHandler.FireProjectile();

                    // if (distanceToPlayer > actionDistance || reloadHandler.IsReloading)
                    //     currentState = EnemyState.Chase;

                    break;
            }

            // Verificação simples de morte (por exemplo, se a saúde chegar a zero)
            // Suponha que você tenha um atributo ou método para verificar a vida atual
            // if(Health <= 0){
            //     currentState = EnemyState.Die;
            //     ExecuteDie();
            // }
        }

        private void ExecuteDie()
        {
            // Instancie o Particle System de morte
            // Exemplo: Instantiate(deathParticlePrefab, transform.position, Quaternion.identity);

            // Instancie o GameObject de EXP para o player
            // Exemplo: Instantiate(expPrefab, transform.position, Quaternion.identity);

            // Retorna o inimigo para a pool de objetos
            enemyPooledObject.ReturnToPool();
        }
    }
}

