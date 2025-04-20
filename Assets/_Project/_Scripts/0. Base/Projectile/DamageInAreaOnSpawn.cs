using UnityEngine;
using GoodVillageGames.Game.Handlers;
using GoodVillageGames.Game.Interfaces;
using GoodVillageGames.Game.Core.Enemy;

namespace GoodVillageGames.Game.Core.Projectiles
{
    public class DamageInAreaOnSpawn : MonoBehaviour, IVisitor
    {
        private float damage;
        [SerializeField] private LayerMask targetLayers;

        void OnTriggerEnter2D(Collider2D other)
        {
            if (((1 << other.gameObject.layer) & targetLayers.value) == 0) return;

            ProcessTarget(other.gameObject);
        }

        void ProcessTarget(GameObject target)
        {
            IVisitable[] visitables = target.GetComponents<IVisitable>();

            foreach (IVisitable visitable in visitables)
            {
                try
                {
                    visitable.Accept(this);
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"Visitor error: {e.Message}");
                }
            }
        }

        public void SetInfo(float damage)
        {
            this.damage = damage;
        }

        public void DisableCollider()
        {
            GetComponent<CircleCollider2D>().enabled = false;
        }

        public void Visit<T>(T visitable) where T : Component, IVisitable
        {

            switch (visitable)
            {
                case HealthHandler health:
                    health.TakeDamage(damage);
                    break;

                case EnemyHealthHandler enemy:
                    enemy.TakeDamage(damage);
                    break;
            }
        }
    }
}