using UnityEngine;
using System.Collections;
using GoodVillageGames.Game.Enums.Pooling;
using GoodVillageGames.Game.Core.MobSpawning;

namespace GoodVillageGames.Game.Core.Manager
{
    public class RegularMobSpawningManager : MobSpawn
    {
        public static RegularMobSpawningManager Instance { get; private set; }

        void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        protected override void SpawnWave(int mobAmountToSpawn)
        {
            currentWaveSpawnPositions.Clear();

            for (int i = 0; i < mobAmountToSpawn; i++)
            {
                GameObject mob = PoolManager.Instance.GetPooledObject(GetRandomMobPool());
                if (mob != null)
                {
                    Vector3 spawnPos = GetValidSpawnPosition();
                    mob.transform.position = spawnPos;
                    MobSpawnAdvisor.Instance.IncrementActiveMobCount();
                    mob.SetActive(true);
                    currentWaveSpawnPositions.Add(spawnPos);
                }
            }
        }

        protected override PoolID GetRandomMobPool()
        {
            return PickRandomPoolID(
            mobSpawnConfig.MobPools,
            entry => entry.Weight,
            entry => entry.PoolID
            );
        }

        protected override IEnumerator SpawnCoroutine()
        {
            // First wave
            float nextSpawnTime = Time.time + (mobSpawnConfig.HasSpawnDelay ? mobSpawnConfig.InitialSpawnDelay : mobSpawnConfig.TimeBetweenWaves);

            while (true)
            {
                // Waiting until it’s time
                yield return new WaitUntil(() => Time.time >= nextSpawnTime);

                // Spawn!
                int desiredMobCount = CalculateMobsPerWave();
                int allowedMobCount = MobSpawnAdvisor.Instance.GetAllowedSpawnCount();
                int mobCountToSpawn = Mathf.Min(desiredMobCount, allowedMobCount);

                if (mobCountToSpawn > 0)
                {
                    SpawnWave(mobCountToSpawn);
                    waveCounter++;
                }

                // Next Wave
                float interval = CalculateSpawnInterval();
                nextSpawnTime += interval;
            }
        }
    }
}