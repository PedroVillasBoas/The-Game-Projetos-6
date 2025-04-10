using UnityEngine;
using GoodVillageGames.Game.Interfaces;
using GoodVillageGames.Game.Core.GameObjectEntity;

namespace GoodVillageGames.Game.Core.Pickup
{
    public abstract class Pickup : MonoBehaviour, IVisitor
    {
        protected abstract void ApplyPickupEffect(Entity entity);

        public void Visit<T>(T visitable) where T : Component, IVisitable
        {
            if (visitable is Entity entity)
            {
                ApplyPickupEffect(entity);
            }
        }

        public void OnTriggerEnter2D(Collider2D collision)
        {
            collision.GetComponent<IVisitable>()?.Accept(this);
            Destroy(gameObject);
        }
    }
}