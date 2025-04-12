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

        public override void DoAction()
        {
            Instantiate(explosionVFXPrefab, transform.position, quaternion.identity);
            base.DoAction();
        }

        protected override void OnCollisionEnter2D(Collision2D collision)
        {
            DoAction();
        }
    }
}