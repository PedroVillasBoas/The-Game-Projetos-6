using UnityEngine;
using TriInspector;
using GoodVillageGames.Game.General;
using GoodVillageGames.Game.Handlers;
using GoodVillageGames.Game.Core.Pooling;
using static GoodVillageGames.Game.Enums.Enums;
using GoodVillageGames.Game.Core.GameObjectEntity;

namespace GoodVillageGames.Game.Core.Enemy.AI
{
    [RequireComponent(typeof(PooledObject))]
    [DeclareFoldoutGroup("Components")]
    [DeclareFoldoutGroup("Prefabs")]
    public class BaseEnemy : Entity
    {
        protected PooledObject enemyPooledObject;
        protected EnemyType enemyType;

        [Title("Enemy Components")]
        [SerializeField, Group("Components")] protected EnemyBaseStats enemyBaseStats;
        [SerializeField, Group("Components")] protected EnemyHealthHandler enemyHealthHandler;
        [SerializeField, Group("Components")] protected EnemyAimHandler enemyAimHandler;
        [SerializeField, Group("Components")] protected EnemyDetectionTrigger enemyDetectionCollider;

        [Title("Enemy Prefabs")]
        [SerializeField, Group("Prefabs")] protected GameObject enemyDeathVFX;

        protected EnemyFireHandler enemyFireHandler;
        protected EnemyReloadHandler enemyReloadHandler;

        private Transform player;

        public Transform Player { get => player; set => player = value; }

        protected override void Awake()
        {
            base.Awake();
            player = GameObject.FindGameObjectWithTag("Player").transform;
            enemyFireHandler = GetComponentInChildren<EnemyFireHandler>();
            enemyReloadHandler = GetComponentInChildren<EnemyReloadHandler>();
            enemyPooledObject = GetComponent<PooledObject>();
        }

        void OnEnable()
        {
            enemyDetectionCollider.Collider.radius = enemyBaseStats.DoActionRadius;
        }

        void Start()
        {
            enemyType = enemyBaseStats.EnemyType;
        }
    }
}
