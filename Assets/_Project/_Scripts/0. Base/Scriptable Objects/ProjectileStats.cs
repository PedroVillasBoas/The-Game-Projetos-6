using System;
using UnityEngine;

namespace GoodVillageGames.Game.Core.Util.Stats
{
    [CreateAssetMenu(fileName = "ProjectileStats", menuName = "Scriptable Objects/Stats/ProjectileStats")]
    public class ProjectileStats : ScriptableObject
    {
        public float ProjectileDamage;
        public float ProjectileLifetime;
        public float ProjectileTravelSpeed;
        public float ProjectileAmount;
        public float ProjectileExplosionRadius;
    }
}

