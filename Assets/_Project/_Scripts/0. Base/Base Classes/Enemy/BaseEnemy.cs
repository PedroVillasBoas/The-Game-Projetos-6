using KBCore.Refs;
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
        protected EnemyType enemyType;
        [Self] protected PooledObject enemyPooledObject;

        [Title("Enemy Components")]
        [SerializeField, Group("Components")] protected EnemyBaseStats enemyBaseStats;
        [SerializeField, Group("Components")] protected EnemyHealthHandler enemyHealthHandler;
        [SerializeField, Group("Components")] protected Transform enemyFirePoint;
        [SerializeField, Group("Components")] protected EnemyAimHandler enemyAimHandler;
        [SerializeField, Group("Components")] protected EnemyFireHandler enemyFireHandler;
        [SerializeField, Group("Components")] protected ReloadHandler enemyReloadHandler;
        [SerializeField, Group("Components")] private CircleCollider2D enemyDetectionCollider;

        [Title("Enemy Prefabs")]
        [SerializeField, Group("Prefabs")] private GameObject enemyDeathVFX;

        private Transform player;

        public Transform Player { get => player; set => player = value; }

        protected override void Awake()
        {
            base.Awake();
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        void OnEnable()
        {
            enemyDetectionCollider.radius = enemyBaseStats.DoActionRadius;
        }

        void Start()
        {
            enemyType = enemyBaseStats.EnemyType;
        }
    }
}
