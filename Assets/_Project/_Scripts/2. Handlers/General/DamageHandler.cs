using UnityEngine;
using GoodVillageGames.Game.Interfaces;
using GoodVillageGames.Game.Core.Projectiles;

namespace GoodVillageGames.Game.Handlers
{
    public class DamageHandler : MonoBehaviour, IVisitor
    {
        private float _damageAmount;

        public void Visit<T>(T visitable) where T : Component, IVisitable
        {
            if (visitable is HealthHandler entity)
            {
                entity.TakeDamage(_damageAmount);
            }
        }

        public void SetDamage(float amount)
        {
            _damageAmount = amount;
        }

        public void OnTriggerEnter2D(Collider2D other)
        {
            other.GetComponent<IVisitable>()?.Accept(this);
            if(TryGetComponent(out BaseProjectile component))
                component.DoAction();
        }
    }
}