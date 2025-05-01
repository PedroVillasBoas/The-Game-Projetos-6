using UnityEngine;
using System.Collections;
using GoodVillageGames.Game.Enums.Pooling;
using GoodVillageGames.Game.Core.MobSpawning;
using System.Linq;

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
            Debug.Log($"Mobs Spawnned this wave: {mobAmountToSpawn}");
            currentWaveSpawnPositions.Clear();

            for (int i = 0; i < mobAmountToSpawn; i++)
            {
                GameObject mob = PoolManager.Instance.GetPooledObject(GetRandomMobPool());
                if (mob != null)
                {
                    Vector3 spawnPos = GetValidSpawnPosition();
                    mob.transform.position = spawnPos;
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
            while (true)
            {
                Debug.Log($"Starting wave {waveCounter + 1} at {Time.time}");
                SpawnWave(CalculateMobsPerWave());
                waveCounter++;
                yield return new WaitForSeconds(CalculateSpawnInterval());
            }
        }
    }
}