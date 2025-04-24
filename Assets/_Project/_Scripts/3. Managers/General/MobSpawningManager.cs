using System.Collections;
using System.Collections.Generic;
using GoodVillageGames.Game.Core.Global;
using TriInspector;
using UnityEngine;
using static GoodVillageGames.Game.Enums.Enums;

namespace GoodVillageGames.Game.Core.Manager
{
    /// <summary>
    /// This class is responsable for controlling the spawn amount and frequency of the mobs and Bosses in a run.
    /// </summary>
    [DeclareFoldoutGroup("Mobs"), DeclareFoldoutGroup("Special Spawn"), DeclareFoldoutGroup("Boss")]
    public class MobSpawningManager : MonoBehaviour
    {
        public static MobSpawningManager Instance { get; private set; }

        [Title("Regular Spawning")]
        [SerializeField, Group("Mobs")] private float initialSpawnInterval = 2f;
        [SerializeField, Group("Mobs")] private float minSpawnInterval = 0.2f;
        [SerializeField, Group("Mobs")] private float spawnRateIncrease = 0.05f;
        [SerializeField, Group("Mobs")] private int baseMobsPerWave = 2;
        [SerializeField, Group("Mobs")] private float mobsIncreasePerMinute = 0.5f;
        [SerializeField, Group("Mobs")] private List<BossPoolEntry> minionPools = new();

        [Title("Circle Spawn")]
        [SerializeField, Group("Special Spawn")] private float circleSpawnCooldown = 10f;
        [SerializeField, Group("Special Spawn")] private int circleMobCount = 20;
        [SerializeField, Group("Special Spawn")] private float circleRadius = 50f;

        [Title("Boss Spawning")]
        [SerializeField, Group("Boss")] private float initialBossSpawnInterval = 180f;
        [SerializeField, Group("Boss")] private float minBossSpawnInterval = 120f;
        [SerializeField, Group("Boss")] private float spawnBossRateIncrease = 0.2f;
        [SerializeField, Group("Boss")] private int baseBossPerWave = 1;
        [SerializeField, Group("Boss")] private float bossIncreasePerCycle = 1f;
        [SerializeField, Group("Boss")] private List<BossPoolEntry> bossPools = new();

        [System.Serializable]
        public struct BossPoolEntry
        {
            public PoolID poolID;
            [Min(1)] public int weight;
        }

        // Local
        private Transform playerTransform;
        private Camera mainCamera;
        private float lastCircleSpawnTime;

        void Awake()
        {
            // Singleton
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        void OnEnable()
        {
            GlobalEventsManager.Instance.ChangeGameStateEventTriggered += OnGameStateChanged;
        }

        void OnDestroy()
        {
            GlobalEventsManager.Instance.ChangeGameStateEventTriggered -= OnGameStateChanged;
        }

        void Start()
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
            mainCamera = Camera.main;
        }

        void OnGameStateChanged(GameState gameState)
        {
            if (gameState == GameState.GameBegin)
                BeginSpawnCoroutines();
        }

        void BeginSpawnCoroutines()
        {
            StartCoroutine(RegularSpawnRoutine());
            StartCoroutine(SpecialSpawnCheckRoutine());
            StartCoroutine(BossSpawnRoutine());
        }

        IEnumerator RegularSpawnRoutine()
        {
            while (true)
            {
                SpawnMobWave(CalculateMobsPerWave());
                yield return new WaitForSeconds(CalculateSpawnInterval());
            }
        }

        IEnumerator SpecialSpawnCheckRoutine()
        {
            while (true)
            {
                if (Time.time - lastCircleSpawnTime > circleSpawnCooldown && Random.value < 0.1f) // 10% chance check
                {
                    SpawnCirclePattern();
                    lastCircleSpawnTime = Time.time;
                }
                yield return new WaitForSeconds(2f);
            }
        }

        int CalculateMobsPerWave()
        {
            float minutes = SceneTimerManager.Instance.GetRunTime() / 60f;
            return baseMobsPerWave + Mathf.FloorToInt(mobsIncreasePerMinute * minutes);
        }

        float CalculateSpawnInterval()
        {
            float time = SceneTimerManager.Instance.GetRunTime();
            return Mathf.Max(minSpawnInterval, initialSpawnInterval - (spawnRateIncrease * time));
        }

        void SpawnMobWave(int mobCount)
        {
            for (int i = 0; i < mobCount; i++)
            {
                SpawnSingleMob();
            }
        }

        void SpawnSingleMob()
        {
            GameObject mob = PoolManager.Instance.GetPooledObject(PoolID.EnemyMinionPrefab);
            if (mob != null)
            {
                Vector3 spawnPos = GetOffscreenSpawnPosition();
                mob.transform.position = spawnPos;
                mob.SetActive(true);
            }
        }

        void SpawnCirclePattern()
        {
            for (int i = 0; i < circleMobCount; i++)
            {
                float angle = i * (360f / circleMobCount);
                Vector3 dir = Quaternion.Euler(0, 0, angle) * Vector3.right;
                Vector3 spawnPos = playerTransform.position + dir * circleRadius;

                GameObject mob = PoolManager.Instance.GetPooledObject(PoolID.EnemyMinionPrefab);
                if (mob != null)
                {
                    mob.transform.position = spawnPos;
                    mob.SetActive(true);
                }
            }
        }

        IEnumerator BossSpawnRoutine()
        {
            // Wait for initial boss spawn time
            yield return new WaitForSeconds(initialBossSpawnInterval);

            while (true)
            {
                SpawnBossWave(CalculateBossPerWave());
                float nextInterval = CalculateBossSpawnInterval();
                yield return new WaitForSeconds(nextInterval);
            }
        }

        int CalculateBossPerWave()
        {
            float cycles = SceneTimerManager.Instance.GetRunTime() / initialBossSpawnInterval;
            return baseBossPerWave + Mathf.FloorToInt(bossIncreasePerCycle * cycles);
        }

        float CalculateBossSpawnInterval()
        {
            float time = SceneTimerManager.Instance.GetRunTime();
            return Mathf.Max(
                minBossSpawnInterval,
                initialBossSpawnInterval - (spawnBossRateIncrease * time)
            );
        }

        void SpawnBossWave(int bossCount)
        {
            if (bossPools.Count == 0)
            {
                Debug.LogWarning("No boss pools configured!");
                return;
            }

            for (int i = 0; i < bossCount; i++)
            {
                SpawnSingleBoss();
            }
        }

        void SpawnSingleBoss()
        {
            PoolID selectedPool = GetRandomBossPool();
            GameObject boss = PoolManager.Instance.GetPooledObject(selectedPool);

            if (boss != null)
            {
                Vector3 spawnPos = GetOffscreenSpawnPosition();
                boss.transform.position = spawnPos;
                boss.SetActive(true);
            }
        }

        PoolID GetRandomBossPool()
        {
            // Calculate total weight
            int totalWeight = 0;
            foreach (var entry in bossPools)
            {
                totalWeight += entry.weight;
            }

            // Select random weighted entry
            int randomValue = Random.Range(0, totalWeight);
            int accumulatedWeight = 0;

            foreach (var entry in bossPools)
            {
                accumulatedWeight += entry.weight;
                if (randomValue < accumulatedWeight)
                {
                    return entry.poolID;
                }
            }

            return bossPools[0].poolID; // Fallback to first entry
        }

        PoolID GetRandomMobPool()
        {
            // Calculate total weight
            int totalWeight = 0;
            foreach (var entry in minionPools)
            {
                totalWeight += entry.weight;
            }

            // Select random weighted entry
            int randomValue = Random.Range(0, totalWeight);
            int accumulatedWeight = 0;

            foreach (var entry in minionPools)
            {
                accumulatedWeight += entry.weight;
                if (randomValue < accumulatedWeight)
                {
                    return entry.poolID;
                }
            }

            return minionPools[0].poolID; // Fallback to first entry
        }

        Vector3 GetOffscreenSpawnPosition()
        {
            Vector2 randomDir = Random.insideUnitCircle.normalized;
            Vector3 spawnPoint = mainCamera.ViewportToWorldPoint(
                new Vector3(
                    randomDir.x < 0 ? 0.1f : 0.9f,
                    randomDir.y < 0 ? 0.1f : 0.9f,
                    mainCamera.nearClipPlane
                )
            );

            // Offset... Just to make sure...
            spawnPoint += (Vector3)randomDir * 3f;
            spawnPoint.z = 0;
            return spawnPoint;
        }
    }
}
