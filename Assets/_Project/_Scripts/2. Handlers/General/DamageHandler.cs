using UnityEngine;
using GoodVillageGames.Game.Core.Util;
using GoodVillageGames.Game.Interfaces;
using GoodVillageGames.Game.Core.Enemy;
using GoodVillageGames.Game.Core.Manager;
using GoodVillageGames.Game.Core.Projectiles;
using GoodVillageGames.Game.Core.Global;

namespace GoodVillageGames.Game.Handlers
{
    public class DamageHandler : MonoBehaviour, IVisitor
    {
        private float _damageAmount;
        private IOwnedProjectile ownedProjectile;

        public float Damage => _damageAmount;

        void Awake() => ownedProjectile = GetComponent<IOwnedProjectile>();

        public void Visit<T>(T visitable) where T : Component, IVisitable
        {
            if (visitable is HealthHandler entity)
            {
                entity.TakeDamage(_damageAmount);
            }

            if (visitable is EnemyHealthHandler entityEnemy)
            {
                entityEnemy.TakeDamage(_damageAmount);
            }

            PoolDamageNumber();
        }

        private void PoolDamageNumber()
        {
            GameObject dmInstance = PoolManager.Instance.GetPooledObject(Enums.Enums.PoolID.DamageNumbers);
            if (dmInstance.TryGetComponent(out DamageNumbers component))
            {
                component.SetInfo(_damageAmount.ToString());
            }
            dmInstance.transform.SetPositionAndRotation(gameObject.transform.position, Quaternion.identity);
            dmInstance.SetActive(true);
        }

        public void SetDamage(float amount)
        {
            _damageAmount = amount;
        }

        public void OnTriggerEnter2D(Collider2D other)
        {
            other.GetComponent<IVisitable>()?.Accept(this);
            if(TryGetComponent(out BaseProjectile component))
            {
                component.DoAction();
                GlobalFileManager.Instance.RegisterHit(ownedProjectile.Type);
            }
        }
    }
}