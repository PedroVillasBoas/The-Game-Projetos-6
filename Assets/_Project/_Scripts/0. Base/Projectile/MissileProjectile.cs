using UnityEngine;
using GoodVillageGames.Game.Core.Pooling;
using TriInspector;
using Unity.Mathematics;

namespace GoodVillageGames.Game.Core.Projectiles
{
    public class MissileProjectile : BaseProjectile
    {
        [Title("Missile Settings")]
        [SerializeField] protected float explosionRadius = 1f;
        [SerializeField] protected GameObject explosionVFXPrefab;

        protected override void Update()
        {
            LaunchProjectile();

            countdown -= Time.deltaTime;
            if (countdown <= 0f)
            {
                MissileExplode();
            }
        }

        protected virtual void MissileExplode()
        {
            Instantiate(explosionVFXPrefab, transform.position, quaternion.identity);

            ReturnToPool();
        }

        protected override void OnCollisionEnter(Collision collision)
        {
            MissileExplode();
        }
    }
}