using UnityEngine;
using TriInspector;
using System.Collections.Generic;
using GoodVillageGames.Game.Enums;
using GoodVillageGames.Game.Enums.Pooling;

namespace GoodVillageGames.Game.Core.ScriptableObjects
{
    [CreateAssetMenu(fileName = "MobSpawnConfig", menuName = "Scriptable Objects/Spawning/MobSpawnConfig")]
    public class MobSpawnConfig : ScriptableObject
    {
        [Title("Spawning Config")]
        public int BaseMobAmount = 2;
        public int BaseMobAdditivePerWave = 1;
        public float SpawnAmountMult = 0.05f;
        public float TimeBetweenWaves = 0.2f;
        public float MinTimeBetweenWaves = 0.2f;
        public float FlatTimeBetweenWavesDecrease = 0.2f;
        public float MinSpawnDistance = 2f;
        public List<MobPoolEntry> MobPools = new();

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