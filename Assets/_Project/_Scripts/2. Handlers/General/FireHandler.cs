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
using GoodVillageGames.Game.Core.Global;

namespace GoodVillageGames.Game.Handlers
{
    public class FireHandler : MonoBehaviour, IFireHandler
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
        private PlayerEventsManager _entityEventsProvider;

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
            _entityEventsProvider = transform.root.GetComponentInChildren<PlayerEventsManager>();
            _entityEventsProvider.OnPlayerBulletEventTriggered.AddListener(UpdateFireInput);
        }

        void OnDisable()
        {
            if (_entityEventsProvider != null)
            {
                _entityEventsProvider.OnPlayerBulletEventTriggered.RemoveListener(UpdateFireInput);
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

        void UpdateFireInput(bool value)
        {
            _inputValue = value;

            if (value && _fireCoroutine == null)
            {
                _fireCoroutine = StartCoroutine(FiringProcess());
            }
            else if (!value && _fireCoroutine != null)
            {
                StopCoroutine(_fireCoroutine);
                _fireCoroutine = null;
            }
        }

        private IEnumerator FiringProcess()
        {
            while (true)
            {
                if (!_reloadHandler.IsReloading)
                {
                    FireProjectile();
                    yield return StartCoroutine(_reloadHandler.Reload());
                }

                if (!_inputValue) break;
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

        public void Fire(PoolID poolId)
        {
            // Getting the pooled projectile from the PoolManager
            GameObject projectile = PoolManager.Instance.GetPooledObject(poolId);
            _entityEventsProvider.PlayerProjectileEvent();
            if (projectile != null)
            {
                if (projectile.TryGetComponent(out BaseProjectile component))
                {
                    component.ProjectileDamageHandler.SetDamage(Damage);
                    GlobalFileManager.Instance.RegisterShot(component.Type);
                }

                projectile.transform.SetPositionAndRotation(_firepoint.position, _firepoint.rotation);
                projectile.SetActive(true);
            }
        }
    }
}
