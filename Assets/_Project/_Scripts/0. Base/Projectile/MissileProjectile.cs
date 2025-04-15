using UnityEngine;
using TriInspector;

namespace GoodVillageGames.Game.Core.Projectiles
{
    public class MissileProjectile : BaseProjectile
    {
        [Title("Missile Settings")]
        [SerializeField] protected float explosionRadius = 1f;

    }
}