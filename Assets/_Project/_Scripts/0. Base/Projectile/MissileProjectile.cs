using UnityEngine;
using TriInspector;
using Unity.Mathematics;

namespace GoodVillageGames.Game.Core.Projectiles
{
    public class MissileProjectile : BaseProjectile
    {
        [Title("Missile Settings")]
        [SerializeField] protected float explosionRadius = 1f;

    }
}