using UnityEngine;
using GoodVillageGames.Game.Interfaces;
using GoodVillageGames.Game.Handlers;
using GoodVillageGames.Game.Core.Pooling;

namespace GoodVillageGames.Game.Core.Pickups
{
    public class HealCurrentHealthPickup : MonoBehaviour, IVisitor
    {

        [SerializeField] private float _healAmount;
        private PooledObject _poolObject;

        void Awake() => _poolObject = GetComponent<PooledObject>();

        public void Visit<T>(T visitable) where T : Component, IVisitable
        {
            if (visitable is HealthHandler entity)
            {
                ApplyHealPickupEffect(entity);
            }
        }

        protected void ApplyHealPickupEffect(HealthHandler handler)
        {
            handler.CurrentHealth += _healAmount;
        }

        public void OnTriggerEnter2D(Collider2D other)
        {
            other.GetComponent<IVisitable>()?.Accept(this);
            _poolObject.ReturnToPool();
        }
    }
}