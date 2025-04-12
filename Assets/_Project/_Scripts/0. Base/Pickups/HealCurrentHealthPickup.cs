using UnityEngine;
using GoodVillageGames.Game.Interfaces;
using GoodVillageGames.Game.Handlers;

namespace GoodVillageGames.Game.Core.Pickup
{
    public class HealCurrentHealthPickup : MonoBehaviour, IVisitor
    {

        [SerializeField] private float _healAmount;

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
            Destroy(gameObject);
        }

    }
}