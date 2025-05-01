using UnityEngine;
using TriInspector;
using System.Collections.Generic;
using GoodVillageGames.Game.Enums;
using GoodVillageGames.Game.Enums.Pooling;

namespace GoodVillageGames.Game.Core.ScriptableObjects
{
    [CreateAssetMenu(fileName = "SpecialSpawnConfig", menuName = "Scriptable Objects/Spawning/SpecialSpawnConfig")]
    [DeclareFoldoutGroup("Special Spawn")]
    public class SpecialMobSpawnConfig : ScriptableObject
    {
        [Title("Circle Spawn")]
        [SerializeField, Group("Special Spawn")] public float CircleSpawnCooldown = 10f;
        [SerializeField, Group("Special Spawn")] public int CircleMobCount = 20;
        [SerializeField, Group("Special Spawn")] public float CircleRadius = 50f;
        [SerializeField, Group("Special Spawn")] public float BaseCircleChance = 0.1f;
        [SerializeField, Group("Special Spawn")] public float ChanceIncreasePerCheck = 0.05f;

        public List<MobPoolEntry> MobPools = new();
        [Space(15)]
        public bool HasSpawnDelay = false;
        [ShowIf("HasSpawnDelay", true)] 
        public float InitialSpawnDelay = 2f;

        // Mob pool config
        [System.Serializable]
        public struct MobPoolEntry
        {
            public PoolID PoolID;
            public GameDifficulty MobDifficulty;
            [Min(1)] public int Weight;
        }
    }
}