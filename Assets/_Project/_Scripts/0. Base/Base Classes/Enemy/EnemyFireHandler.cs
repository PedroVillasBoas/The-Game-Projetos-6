using UnityEngine;
using TriInspector;
using System.Collections;
using GoodVillageGames.Game.Interfaces;
using GoodVillageGames.Game.Core.Manager;
using GoodVillageGames.Game.Core.Attributes;
using GoodVillageGames.Game.Core.Projectiles;
using static GoodVillageGames.Game.Enums.Enums;
using GoodVillageGames.Game.Core.Manager.Player;
using GoodVillageGames.Game.Core.GameObjectEntity;
using GoodVillageGames.Game.Core.Enemy.AI;

namespace GoodVillageGames.Game.Handlers
{
    public class EnemyFireHandler : MonoBehaviour, IFireHandler
    {
        [Title("Components")]
        [SerializeField] private Transform _firepoint;

        [Title("Projectile Pool")]
        [SerializeField] private PoolID _poolID;

        private IAimHandler _aimHandler;
        private Stats _entityStats;
        private IReloadHandler _reloadHandler;
        private bool _inputValue = false;
        private Coroutine _fireCoroutine;
        private EnemyAI _entityEventsProvider;

        public float Damage { get => _entityStats.BaseAttackDamage; }

        public IAimHandler AimHandler { get => _aimHandler; set => _aimHandler = value; }
        public IReloadHandler ReloadHandler { get => _reloadHandler; set => _reloadHandler = value; }
        public Transform Firepoint { get => _firepoint; set => _firepoint = value; }
        public PoolID ProjectilePoolID { get => _poolID; set => _poolID = value; }
        public Coroutine FireCoroutine { get => _fireCoroutine; set => _fireCoroutine = value; }

        void Awake()
        {
            _reloadHandler = GetComponent<ReloadHandler>();
        }

        void OnEnable()
        {
            _entityEventsProvider = transform.root.GetComponentInChildren<EnemyAI>();
            _entityEventsProvider.DoActionEventTriggered += UpdateEnemyFire;
        }

        void OnDisable()
        {
            if (_entityEventsProvider != null)
            {
                _entityEventsProvider.DoActionEventTriggered -= UpdateEnemyFire;
            }
        }

        void Start()
        {
            if (transform.root.TryGetComponent<Entity>(out var statsProvider))
            {
                _entityStats = statsProvider.Stats;
            }
            else
                Debug.LogError("IStatsProvider component not found on entity!", this);
        }

        void UpdateEnemyFire(EnemyAI enemy)
        {
            _inputValue = true;

            if (_fireCoroutine == null)
            {
                _fireCoroutine = StartCoroutine(FiringProcess());
            }

        }

        private IEnumerator FiringProcess()
        {
            while (_inputValue)
            {
                if (!_reloadHandler.IsReloading)
                {
                    FireProjectile();
                    yield return StartCoroutine(_reloadHandler.Reload());
                }
                // Wating a frame if reloading
                else
                    yield return null;
            }
            _fireCoroutine = null;
        }

        public void FireProjectile()
        {
            if (!_reloadHandler.IsReloading)
            {
                Fire(_poolID);
            }
        }

        public void StopFiring()
        {
            _inputValue = false;
            if (_fireCoroutine != null)
            {
                StopCoroutine(_fireCoroutine);
                _fireCoroutine = null;
            }
        }

        public void Fire(PoolID poolId)
        {
            // Getting the pooled projectile from the PoolManager
            GameObject projectile = PoolManager.Instance.GetPooledObject(poolId);
            if (projectile != null)
            {
                if (projectile.TryGetComponent(out BaseProjectile component))
                {
                    component.ProjectileDamageHandler.SetDamage(Damage);
                }

                projectile.transform.SetPositionAndRotation(_firepoint.position, _firepoint.rotation);
                projectile.SetActive(true);
            }
        }
    }
}
