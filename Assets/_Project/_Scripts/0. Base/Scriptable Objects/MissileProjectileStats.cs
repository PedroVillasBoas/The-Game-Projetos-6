using UnityEngine;

namespace GoodVillageGames.Game.Core.Util.Stats
{
    [CreateAssetMenu(fileName = "MissileStats", menuName = "Scriptable Objects/Stats/MissileStats")]
    public class MissileProjectileStats : ProjectileStats
    {
        public float MissileCooldown;
    }
}

