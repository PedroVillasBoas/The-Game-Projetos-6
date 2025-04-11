using UnityEngine;
using TriInspector;
using GoodVillageGames.Game.Core.Pooling;
using GoodVillageGames.Game.Core.Util.Timer;

namespace GoodVillageGames.Game.Core.Projectiles
{
    public class BaseProjectile : MonoBehaviour
    {
        [Title("Projectile Settings")]
        [SerializeField] protected float speed = 10f;
        [SerializeField] protected float lifeTime = 5f;

        private CountdownTimer _timer;
        private Rigidbody2D _projectileRb;

        
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
            if (_timer != null) 
            {
                _timer.Reset(lifeTime);
                _timer.Start();
            }
            else 
                CreateTimer();
        }

        protected virtual void Update()
        {
            LaunchProjectile();

            if (_timer != null)
                _timer.Tick(Time.deltaTime);
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

        protected virtual void DoAction()
        {
            _timer.Stop();
            pooledObject.ReturnToPool();
        }

        protected virtual void OnCollisionEnter2D(Collision2D collision)
        {
            // I'll add the collision and damage stuff later
            DoAction();
        }
    }
}
