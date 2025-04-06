using UnityEngine;
using GoodVillageGames.Game.Core.Pooling;

namespace GoodVillageGames.Game.Core.Projectiles
{
    public class BaseProjectile : MonoBehaviour
    {
        [Header("Projectile Settings")]
        [SerializeField] private float speed = 10f;
        [SerializeField] private float lifeTime = 5f;

        private Rigidbody2D _projectileRb;

        private float _timer;
        private PooledObject _pooledObject;

        protected virtual void Awake()
        {
            _projectileRb = GetComponent<Rigidbody2D>();
            _pooledObject = GetComponent<PooledObject>();
            if (_pooledObject == null)
            {
                Debug.LogWarning("PooledObject component not found on BaseProjectile. Adding one automatically.");
                _pooledObject = gameObject.AddComponent<PooledObject>();
            }
        }

        protected virtual void OnEnable()
        {
            // Reset the lifetime timer when projectile is (re)enabled
            _timer = lifeTime;
        }

        protected virtual void Update()
        {
            LaunchProjectile();

            // Countdown the life timer.
            _timer -= Time.deltaTime;
            if (_timer <= 0f)
            {
                ReturnToPool();
            }
        }

        protected virtual void LaunchProjectile()
        {
            _projectileRb.linearVelocity = speed * transform.up;
        }

        public virtual void ReturnToPool()
        {
            _pooledObject.ReturnToPool();
        }

        protected virtual void OnCollisionEnter(Collision collision)
        {
            // I'll add the collision stuff later
            ReturnToPool();
        }
    }
}
