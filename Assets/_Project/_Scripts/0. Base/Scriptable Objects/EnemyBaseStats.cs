using GoodVillageGames.Game.General;
using TriInspector;
using UnityEngine;
using static GoodVillageGames.Game.Enums.Enums;

namespace GoodVillageGames.Game.General
{
    [CreateAssetMenu(fileName = "EnemyStats", menuName = "Scriptable Objects/Stats/EnemyStats")]
    [DeclareFoldoutGroup("Enemy Stats")]
    public class EnemyBaseStats : BaseStats
    {
        [Title("Enemy Stats")]
        [Group("Enemy Stats")] public float DoActionRadius = 100f;
        [Group("Enemy Stats")] public  EnemyType EnemyType;
    }
}
