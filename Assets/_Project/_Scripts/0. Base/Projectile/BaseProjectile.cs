using UnityEngine;
using TriInspector;
using GoodVillageGames.Game.Handlers;
using GoodVillageGames.Game.Interfaces;
using GoodVillageGames.Game.Core.Pooling;
using GoodVillageGames.Game.Core.Util.Timer;
using GoodVillageGames.Game.Enums.Projectiles;

namespace GoodVillageGames.Game.Core.Projectiles
{
    public class BaseProjectile : MonoBehaviour, IOwnedProjectile
    {
        [Title("Projectile Settings")]
        [SerializeField] protected float speed = 10f;
        [SerializeField] protected float lifeTime = 5f;
        [SerializeField] protected GameObject hitVFXPrefab;
        [SerializeField] private ProjectileType projectileType;

        private CountdownTimer _timer;
        private Rigidbody2D _projectileRb;
        private DamageHandler _damageHandler;

        protected PooledObject pooledObject;

        public DamageHandler ProjectileDamageHandler { get => _damageHandler; }
        public ProjectileType Type { get => projectileType; protected set => projectileType = value; }
        public bool IsPlayerProjectile => Type == ProjectileType.Basic || Type == ProjectileType.Missile;

        protected virtual void Awake()
        {
            _projectileRb = GetComponent<Rigidbody2D>();
            pooledObject = GetComponent<PooledObject>();
            _damageHandler = GetComponent<DamageHandler>();

            if (pooledObject == null)
            {
                Debug.LogWarning("PooledObject component not found on BaseProjectile. Adding one automatically.");
                pooledObject = gameObject.AddComponent<PooledObject>();
            }
        }

        protected virtual void OnEnable() => CreateTimer();

        protected virtual void Update()
        {
            LaunchProjectile();

            _timer?.Tick(Time.deltaTime);
        }

        protected void CreateTimer()
        {
            _timer = new CountdownTimer(lifeTime);
            _timer.OnTimerStop += () => DoAction();
            _timer.Start();
        }

        protected virtual void LaunchProjectile()
        {
            _projectileRb.linearVelocity = speed * transform.up;
        }

        public virtual void DoAction()
        {
            if (_timer != null) _timer = null;

            Instantiate(hitVFXPrefab, transform.position, Quaternion.identity);
            pooledObject.ReturnToPool();
        }

        protected virtual void OnCollisionEnter2D(Collision2D collision)
        {
            DoAction();
        }
    }
}
