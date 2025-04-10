using UnityEngine;
using TriInspector;

namespace GoodVillageGames.Game.General
{
    [CreateAssetMenu(fileName = "PlayerStats", menuName = "Scriptable Objects/Stats/BaseStats")]
    [DeclareFoldoutGroup("Base Stats")]
    public class BaseStats : ScriptableObject
    {
        [Title("Base Info")]
        public string Name = "Name";

        [Title("Visuals Prefab")]
        public GameObject EntityPrefab;

        [Title("Base Stats")]
        [Group("Base Stats")] public float MaxHealth = 100f;
        [Group("Base Stats")] public float MaxSpeed = 20f;
        [Group("Base Stats")] public float MaxDefense = 20f;
        [Group("Base Stats")] public float Acceleration = 10f;

        [Title("Basic Attack")]
        [Group("Base Stats")] public float BaseAttackDamage = 5f;
        [Group("Base Stats")] public float AttackSpeed = 1f;

        [Title("Boost")]
        [Group("Base Stats")] public float MaxBoostTime = 3f;
        [Group("Base Stats")] public float MaxBoostSpeed = 30f;

        [Title("Missile")]
        [Group("Base Stats")] public float BaseMissileDamage = 10f;
        [Group("Base Stats")] public float BaseMissileCooldown = 10f;
        [Group("Base Stats")] public float MaxMissileAmmo = 3f;
    }
}
