using UnityEngine;
using TriInspector;

namespace GoodVillageGames.Game.General
{
    [CreateAssetMenu(fileName = "PlayerStats", menuName = "Scriptable Objects/Stats/PlayerStats")]
    public class PlayerStats : Stats
    {
            [Title("Passive")]
            public string PassiveName;
            public string PassiveDescription;

            [Title("Player Stats")]
            public float Acceleration;
            
            [Title("Basic Attack")]
            public int AttackDamage;
            public float AttackSpeed;
            public GameObject BulletPrefab;

            [Title("Missile")]
            public int MissileDamage;
            public float MissileCooldown;
            public float MissileAttackSpeed;
            public GameObject MissilePrefab;
    }
}
