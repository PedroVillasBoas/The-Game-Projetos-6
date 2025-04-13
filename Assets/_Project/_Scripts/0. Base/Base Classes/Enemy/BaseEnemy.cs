using KBCore.Refs;
using UnityEngine;
using GoodVillageGames.Game.General;
using static GoodVillageGames.Game.Enums.Enums;
using GoodVillageGames.Game.Core.GameObjectEntity;
using GoodVillageGames.Game.Handlers;
using GoodVillageGames.Game.Core.Pooling;

namespace GoodVillageGames.Game.Core.Enemy.AI
{
    [RequireComponent(typeof(PooledObject))]
    public class BaseEnemy : Entity
    {
        protected EnemyType enemyType;
        [Self] protected PooledObject enemyPooledObject;
        [SerializeField] protected EnemyBaseStats enemyBaseStats;
        // [SerializeField] protected HealthHandler enemyHealthHandler;

        private Transform player;
        [SerializeField] private CircleCollider2D enemyCollider;

        public Transform Player { get => player; set => player = value; }

        protected override void Awake()
        {
            base.Awake();

            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        void OnEnable()
        {

        }

        void Start()
        {
            enemyType = enemyBaseStats.EnemyType;
        }
    }
}