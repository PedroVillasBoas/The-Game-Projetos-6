using System;
using UnityEngine;
using TriInspector;
using System.Collections;
using GoodVillageGames.Game.Interfaces;
using GoodVillageGames.Game.Core.Manager.Player;

namespace GoodVillageGames.Game.Handlers
{
    public class FireHandler : MonoBehaviour, IFireHandler
    {
        [Title("Components")]
        [SerializeField] private Transform _firepoint;

        [Title("Projectile")]
        [SerializeField] private GameObject _projectilePrefab;

        private IAimHandler _aimHandler;
        private ReloadHandler _reloadHandler;
        private bool _inputValue = false;
        private Coroutine _fireCoroutine;

        public IAimHandler AimHandler { get => _aimHandler; set => _aimHandler = value; }
        public ReloadHandler ReloadHandler { get => _reloadHandler; set => _reloadHandler = value; }
        public Transform Firepoint { get => _firepoint; set => _firepoint = value; }
        public GameObject ProjectilePrefab { get => _projectilePrefab; set => _projectilePrefab = value; }
        public Coroutine FireCoroutine { get => _fireCoroutine; set => _fireCoroutine = value; }

        void Awake()
        {
            _reloadHandler = GetComponent<ReloadHandler>();
        }

        void OnEnable()
        {
            var entityEventsProvider = transform.root.GetComponentInChildren<PlayerEventsManager>();
            if (entityEventsProvider != null)
            {
                entityEventsProvider.OnPlayerBulletEventTriggered.AddListener(UpdateFireInput);
            }
        }

        void OnDisable()
        {
            var entityEventsProvider = transform.root.GetComponentInChildren<PlayerEventsManager>();
            if (entityEventsProvider != null)
            {
                entityEventsProvider.OnPlayerBulletEventTriggered.RemoveListener(UpdateFireInput);
            }
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
                //Fire(_projectilePrefab);
                StartCoroutine(_reloadHandler.Reload());
            }
        }

        // public void Fire(GameObject prefab)
        // {
        //     GameObject projectile = ProjectilePoolManager.Instance.GetPooledObject(prefab);
        //     if (projectile != null)
        //     {
        //         projectile.transform.position = _firepoint.position;
        //         projectile.transform.rotation = _firepoint.rotation;
        //         projectile.SetActive(true);

        //         if (projectile.TryGetComponent<Projectile>(out var projectileComponent))
        //         {
        //             projectileComponent.Initialize(new StatsHolder(), Vector2.zero); // THIS IS WRONG!!!!
        //         }
        //     }
        // }
    }
}
