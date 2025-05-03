using UnityEngine;
using System.Collections;
using GoodVillageGames.Game.Enums.Pooling;
using GoodVillageGames.Game.Core.MobSpawning;

namespace GoodVillageGames.Game.Core.Manager
{
    public class SpecialMobSpawningManager : MobSpawn
    {
        public static SpecialMobSpawningManager Instance { get; private set; }

        private float currentCircleChance;

        void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        void Start() => currentCircleChance = specialMobSpawnConfig.BaseCircleChance;

        protected override PoolID GetRandomMobPool()
        {
            return PickRandomPoolID(
            specialMobSpawnConfig.MobPools,
            entry => entry.Weight,
            entry => entry.PoolID
            );
        }

        protected override void SpawnWave(int mobAmountToSpawn)
        {
            for (int i = 0; i < specialMobSpawnConfig.CircleMobCount; i++)
            {
                float angle = i * (360f / specialMobSpawnConfig.CircleMobCount);
                Vector3 dir = Quaternion.Euler(0, 0, angle) * Vector3.right;
                Vector3 spawnPos = playerTransform.position + dir * specialMobSpawnConfig.CircleRadius;

                GameObject mob = PoolManager.Instance.GetPooledObject(GetRandomMobPool());
                if (mob != null)
                {
                    mob.transform.position = spawnPos;
                    mob.SetActive(true);
                }
            }
        }

        protected override IEnumerator SpawnCoroutine()
        {
            var config = specialMobSpawnConfig;
            currentCircleChance = config.BaseCircleChance;

            // First wave
            float nextSpawnTime = Time.time + (config.HasSpawnDelay ? config.InitialSpawnDelay : config.CircleSpawnCooldown);

            while (true)
            {
                while (Time.time < nextSpawnTime)
                    yield return null;

                // Try to Spawn!
                if (Random.value < currentCircleChance)
                {
                    SpawnWave(config.CircleMobCount);
                    currentCircleChance = config.BaseCircleChance;
                }
                else
                {
                    currentCircleChance = Mathf.Clamp01(currentCircleChance + config.ChanceIncreasePerCheck);
                }
                nextSpawnTime += config.CircleSpawnCooldown;
            }
        }
    }
}