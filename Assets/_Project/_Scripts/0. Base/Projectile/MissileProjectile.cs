using UnityEngine;
using TriInspector;
using Unity.Mathematics;

namespace GoodVillageGames.Game.Core.Projectiles
{
    public class MissileProjectile : BaseProjectile
    {
        [Title("Missile Settings")]
        [SerializeField] protected float explosionRadius = 1f;

        public override void DoAction()
        {
            base.DoAction();
        }

        protected override void OnCollisionEnter2D(Collision2D collision)
        {
            DoAction();
        }
    }
}