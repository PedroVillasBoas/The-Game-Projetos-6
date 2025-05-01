using UnityEngine;
using TriInspector;
using GoodVillageGames.Game.Enums.Enemy;

namespace GoodVillageGames.Game.General
{
    [CreateAssetMenu(fileName = "EnemyStats", menuName = "Scriptable Objects/Stats/EnemyStats")]
    [DeclareFoldoutGroup("Enemy Stats")]
    public class EnemyBaseStats : BaseStats
    {
        [Title("Enemy Stats")]
        [Group("Enemy Stats")] public float DoActionRadius = 100f;
        [Group("Enemy Stats")] public EnemyType EnemyType;
    }
}
