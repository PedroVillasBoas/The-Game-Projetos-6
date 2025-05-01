using System;
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