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
using GoodVillageGames.Game.Core;

namespace GoodVillageGames.Game.Handlers
{
    public class MissileFireHandler : MonoBehaviour, IFireHandler
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

        public float Damage { get => _entityStats.BaseMissileDamage; }

        public IAimHandler AimHandler { get => _aimHandler; set => _aimHandler = value; }
        public IReloadHandler ReloadHandler { get => _reloadHandler; set => _reloadHandler = value; }
        public Transform Firepoint { get => _firepoint; set => _firepoint = value; }
        public PoolID ProjectilePoolID { get => _poolID; set => _poolID = value; }
        public Coroutine FireCoroutine { get => _fireCoroutine; set => _fireCoroutine = value; }

        void Awake()
        {
            _reloadHandler = GetComponent<MissileReloadHandler>();
        }

        void OnEnable()
        {
            _entityEventsProvider = transform.root.GetComponentInChildren<PlayerEventsManager>();
            
            _entityEventsProvider.OnPlayerMissileEventTriggered.AddListener(UpdateFireMissileInput);
        }


        void OnDisable()
        {
            if (_entityEventsProvider != null)
            {
                _entityEventsProvider.OnPlayerMissileEventTriggered.RemoveListener(UpdateFireMissileInput);
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
        
        void UpdateFireMissileInput(bool value)
        {
            _inputValue = value;

            if (value && _fireCoroutine == null)
            {
                _fireCoroutine = StartCoroutine(MissileFiringProcess());
            }
            else if (!value && _fireCoroutine != null)
            {
                StopCoroutine(_fireCoroutine);
                _fireCoroutine = null;
            }
        }

        private IEnumerator MissileFiringProcess()
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
                Fire(_poolID);
            else
                PlayerAudioHandler.Instance.PlayPlayerMissileOnCooldownShootSFX();
        }

        private void Fire(PoolID poolID)
        {
            GameObject projectile = PoolManager.Instance.GetPooledObject(poolID);
            _entityEventsProvider.PlayerProjectileEvent();
            if (projectile != null)
            {
                if (projectile.TryGetComponent(out BaseProjectile component))
                {
                    component.ProjectileDamageHandler.SetDamage(Damage);
                    GlobalFileManager.Instance.RegisterShot(component.Type);
                    PlayerAudioHandler.Instance.PlayPlayerMissileShootSFX();
                }
                
                projectile.transform.SetPositionAndRotation(_firepoint.position, _firepoint.rotation);
                projectile.SetActive(true);
            }
        }
    }
}
