using UnityEngine;
using System.Collections;
using GoodVillageGames.Game.Enums.Pooling;
using GoodVillageGames.Game.Core.MobSpawning;

namespace GoodVillageGames.Game.Core.Manager
{
    public class RegularMobSpawningManager : MobSpawn
    {
        public static RegularMobSpawningManager Instance { get; private set; }

        // Coroutines
        private Coroutine regularSpawnRoutine;

        // Flag
        private bool isSpawning;

        void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        protected override void BeginSpawnCoroutine()
        {
            throw new System.NotImplementedException();
        }

        protected override PoolID GetRandomMobPool()
        {
            throw new System.NotImplementedException();
        }

        protected override IEnumerator SpawnRoutine()
        {
            throw new System.NotImplementedException();
        }

        protected override void SpawnWave(int mobAmountToSpawn)
        {
            throw new System.NotImplementedException();
        }
    }
}