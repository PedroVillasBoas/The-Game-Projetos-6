using UnityEngine;
using System.Collections;
using GoodVillageGames.Game.Enums.Pooling;
using GoodVillageGames.Game.Core.MobSpawning;

namespace GoodVillageGames.Game.Core.Manager
{
    public class BossSpawningManager : MobSpawn
    {
        public static BossSpawningManager Instance { get; private set; }

        void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        protected override PoolID GetRandomMobPool()
        {
            return PickRandomPoolID(
            mobSpawnConfig.MobPools,
            entry => entry.Weight,
            entry => entry.PoolID
            );
        }

        protected override void SpawnWave(int mobAmountToSpawn)
        {
            Debug.Log($"Bosses Spawnned this wave: {mobAmountToSpawn}");

            for (int i = 0; i < mobAmountToSpawn; i++)
            {
                GameObject Bosses = PoolManager.Instance.GetPooledObject(GetRandomMobPool());
                if (Bosses != null)
                {
                    Vector3 spawnPos = GetValidSpawnPosition();
                    Bosses.transform.position = spawnPos;
                    Bosses.SetActive(true);
                }
            }
        }

        protected override IEnumerator SpawnCoroutine()
        {
            // First wave
            float nextSpawnTime = Time.time + (mobSpawnConfig.HasSpawnDelay ? mobSpawnConfig.InitialSpawnDelay : mobSpawnConfig.TimeBetweenWaves);

            while (true)
            {
                // Waiting until it’s time
                while (Time.time < nextSpawnTime)
                    yield return null;

                // Spawn!
                int mobCount = CalculateMobsPerWave();
                SpawnWave(mobCount);
                waveCounter++;

                // Next Wave
                float interval = CalculateSpawnInterval();
                nextSpawnTime += interval;
            }
        }
    }
}