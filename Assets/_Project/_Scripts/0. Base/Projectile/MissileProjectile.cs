using UnityEngine;
using TriInspector;

namespace GoodVillageGames.Game.Core.Projectiles
{
    public class MissileProjectile : BaseProjectile
    {
        [Title("Missile Settings")]
        [SerializeField] protected float explosionRadius = 1f;

        public override void DoAction()
        {
            var instance = Instantiate(hitVFXPrefab, transform.position, Quaternion.identity);
            if (instance.TryGetComponent(out DamageInAreaOnSpawn damageArea))
            {
                damageArea.SetInfo(ProjectileDamageHandler.Damage);
            }

            pooledObject.ReturnToPool();
        }

    }
}