using System;
using UnityEngine;
using TriInspector;
using System.Collections;
using GoodVillageGames.Game.Interfaces;
using GoodVillageGames.Game.Core.Manager;
using static GoodVillageGames.Game.Enums.Enums;
using GoodVillageGames.Game.Core.Manager.Player;
using GoodVillageGames.Game.Handlers.UI;
using GoodVillageGames.Game.Core.Manager.UI;

namespace GoodVillageGames.Game.Handlers
{
    public class MissileFireHandler : MonoBehaviour, IFireHandler
    {
        [Title("Components")]
        [SerializeField] private Transform _firepoint;

        [Title("Projectile Pool")]
        [SerializeField] private PoolID _poolID;

        private IAimHandler _aimHandler;
        private IReloadHandler _reloadHandler;
        private bool _inputValue = false;
        private Coroutine _fireCoroutine;

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
            var entityEventsProvider = transform.root.GetComponentInChildren<PlayerEventsManager>();
            
            entityEventsProvider.OnPlayerMissileEventTriggered.AddListener(UpdateFireMissileInput);
        }


        void OnDisable()
        {
            var entityEventsProvider = transform.root.GetComponentInChildren<PlayerEventsManager>();
            if (entityEventsProvider != null)
            {
                entityEventsProvider.OnPlayerMissileEventTriggered.RemoveListener(UpdateFireMissileInput);
            }
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
            {
                Fire(_poolID);
            }
        }

        private void Fire(PoolID poolID)
        {
            GameObject projectile = PoolManager.Instance.GetPooledObject(poolID);
            if (projectile != null)
            {
                projectile.transform.SetPositionAndRotation(_firepoint.position, _firepoint.rotation);
                projectile.SetActive(true);
            }
        }
    }
}
