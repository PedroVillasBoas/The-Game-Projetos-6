using UnityEngine;
using TriInspector;
using GoodVillageGames.Game.Core.Pooling;

namespace GoodVillageGames.Game.Core.Projectiles
{
    public class BaseProjectile : MonoBehaviour
    {
        [Title("Projectile Settings")]
        [SerializeField] protected float speed = 10f;
        [SerializeField] protected float lifeTime = 5f;

        private Rigidbody2D _projectileRb;

        protected float countdown;
        protected PooledObject pooledObject;

        protected virtual void Awake()
        {
            _projectileRb = GetComponent<Rigidbody2D>();
            pooledObject = GetComponent<PooledObject>();
            if (pooledObject == null)
            {
                Debug.LogWarning("PooledObject component not found on BaseProjectile. Adding one automatically.");
                pooledObject = gameObject.AddComponent<PooledObject>();
            }
        }

        protected virtual void OnEnable()
        {
            // Reset the lifetime timer when projectile is (re)enabled
            countdown = lifeTime;
        }

        protected virtual void Update()
        {
            LaunchProjectile();

            // Countdown the life timer.
            countdown -= Time.deltaTime;
            if (countdown <= 0f)
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
            pooledObject.ReturnToPool();
        }

        protected virtual void OnCollisionEnter(Collision collision)
        {
            // I'll add the collision and damage stuff later
            ReturnToPool();
        }
    }
}
