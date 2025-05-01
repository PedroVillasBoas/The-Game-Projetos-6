using UnityEngine;
using System.Linq;
using TriInspector;
using System.Collections;
using System.Collections.Generic;
using GoodVillageGames.Game.Enums;
using GoodVillageGames.Game.Core.Global;
using GoodVillageGames.Game.Enums.Pooling;

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
        [SerializeField, Group("Mobs")] private float mobsIncreasePerWave = 0.5f;
        [SerializeField, Group("Mobs")] private float minSpawnDistance = 2f;
        [SerializeField, Group("Mobs")] private List<MinionPoolEntry> minionPools = new();

        [Title("Circle Spawn")]
        [SerializeField, Group("Special Spawn")] private float circleSpawnCooldown = 10f;
        [SerializeField, Group("Special Spawn")] private int circleMobCount = 20;
        [SerializeField, Group("Special Spawn")] private float circleRadius = 50f;
        [SerializeField, Group("Special Spawn")] private float baseCircleChance = 0.1f;
        [SerializeField, Group("Special Spawn")] private float chanceIncreasePerCheck = 0.05f;

        [Title("Boss Spawning")]
        [SerializeField, Group("Boss")] private float initialBossSpawnInterval = 180f;
        [SerializeField, Group("Boss")] private float minBossSpawnInterval = 120f;
        [SerializeField, Group("Boss")] private int baseBossPerWave = 1;
        [SerializeField, Group("Boss")] private float bossIntervalDecreasePerWave = 5f;
        [SerializeField, Group("Boss")] private int minBossIncrease = 1;
        [SerializeField, Group("Boss")] private int maxBossIncrease = 2;
        [SerializeField, Group("Boss")] private List<BossPoolEntry> bossPools = new();

        [System.Serializable]
        public struct BossPoolEntry
        {
            public PoolID poolID;
            [Min(1)] public int weight;
        }

        [System.Serializable]
        public struct MinionPoolEntry
        {
            public PoolID poolID;
            [Min(1)] public int weight;
        }

        // Local variables
        private Transform playerTransform;
        private Camera mainCamera;
        private float lastCircleSpawnTime;
        private float currentCircleChance;
        private int waveCounter;
        private int bossWaveCounter;
        private int currentBossWaveTier;
        private List<Vector3> currentWaveSpawnPositions = new();

        // Coroutines
        private Coroutine regularSpawnRoutine;
        private Coroutine specialSpawnRoutine;
        private Coroutine bossSpawnRoutine;

        // Flag
        private bool isSpawning;

        void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        void OnEnable() => GlobalEventsManager.Instance.ChangeGameStateEventTriggered += OnGameStateChanged;
        void OnDestroy() => GlobalEventsManager.Instance.ChangeGameStateEventTriggered -= OnGameStateChanged;

        void Start()
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
            mainCamera = Camera.main;
            currentCircleChance = baseCircleChance;
        }

        void OnGameStateChanged(GameState gameState)
        {
            if (gameState == GameState.GameBegin)
                BeginSpawnCoroutines();
        }

        void BeginSpawnCoroutines()
        {
            if(isSpawning) return;
            
            // Stopping any existing coroutines
            if(regularSpawnRoutine != null) StopCoroutine(regularSpawnRoutine);
            if(specialSpawnRoutine != null) StopCoroutine(specialSpawnRoutine);
            if(bossSpawnRoutine != null) StopCoroutine(bossSpawnRoutine);
            
            // Starting new coroutines
            regularSpawnRoutine = StartCoroutine(RegularSpawnRoutine());
            specialSpawnRoutine = StartCoroutine(SpecialSpawnCheckRoutine());
            bossSpawnRoutine = StartCoroutine(BossSpawnRoutine());
            
            isSpawning = true;
        }

        IEnumerator RegularSpawnRoutine()
        {
            while (true)
            {
                Debug.Log($"Starting wave {waveCounter + 1} at {Time.time}");
                SpawnMobWave(CalculateMobsPerWave());
                waveCounter++;
                yield return new WaitForSeconds(CalculateSpawnInterval());
            }
        }

        IEnumerator SpecialSpawnCheckRoutine()
        {
            while (true)
            {
                bool cooldownPassed = Time.time - lastCircleSpawnTime > circleSpawnCooldown;

                if (cooldownPassed)
                {
                    if (Random.value < currentCircleChance)
                    {
                        SpawnCirclePattern();
                        lastCircleSpawnTime = Time.time;
                        currentCircleChance = baseCircleChance;
                    }
                    else
                    {
                        currentCircleChance += chanceIncreasePerCheck;
                    }
                }

                yield return new WaitForSeconds(circleSpawnCooldown);
            }
        }

        int CalculateMobsPerWave()
        {
            return baseMobsPerWave + Mathf.FloorToInt(mobsIncreasePerWave * waveCounter);
        }

        float CalculateSpawnInterval()
        {
            float time = SceneTimerManager.Instance.GetRunTime();
            return Mathf.Max(minSpawnInterval, initialSpawnInterval - (spawnRateIncrease * time));
        }

        void SpawnMobWave(int mobCount)
        {
            Debug.Log($"Mobs Spawnned this wave: {mobCount}");
            currentWaveSpawnPositions.Clear();

            for (int i = 0; i < mobCount; i++)
            {
                SpawnSingleMob();
            }
        }

        void SpawnSingleMob()
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

        Vector3 GetValidSpawnPosition()
        {
            Vector3 spawnPos;
            int attempts = 0;
            bool validPosition;

            do
            {
                validPosition = true;
                spawnPos = GetOffscreenSpawnPosition();

                foreach (var pos in currentWaveSpawnPositions)
                {
                    if (Vector3.Distance(spawnPos, pos) < minSpawnDistance)
                    {
                        validPosition = false;
                        break;
                    }
                }

                var colliders = Physics2D.OverlapCircleAll(spawnPos, minSpawnDistance);
                if (colliders.Any(c => c.CompareTag("Enemy")))
                {
                    validPosition = false;
                }

                attempts++;
            } while (!validPosition && attempts < 10);

            return spawnPos;
        }

        void SpawnCirclePattern()
        {
            for (int i = 0; i < circleMobCount; i++)
            {
                float angle = i * (360f / circleMobCount);
                Vector3 dir = Quaternion.Euler(0, 0, angle) * Vector3.right;
                Vector3 spawnPos = playerTransform.position + dir * circleRadius;

                GameObject mob = PoolManager.Instance.GetPooledObject(GetRandomMobPool());
                if (mob != null)
                {
                    mob.transform.position = spawnPos;
                    mob.SetActive(true);
                }
            }
        }

        IEnumerator BossSpawnRoutine()
        {
            yield return new WaitForSeconds(initialBossSpawnInterval);

            while (true)
            {
                SpawnBossWave(CalculateBossPerWave());
                float nextInterval = CalculateBossSpawnInterval();
                bossWaveCounter++;
                yield return new WaitForSeconds(nextInterval);
            }
        }

        int CalculateBossPerWave()
        {
            currentBossWaveTier++;
            int randomIncrease = Random.Range(
                minBossIncrease + currentBossWaveTier,
                maxBossIncrease + currentBossWaveTier
            );
            return baseBossPerWave + randomIncrease;
        }

        float CalculateBossSpawnInterval()
        {
            return Mathf.Max(
                minBossSpawnInterval,
                initialBossSpawnInterval - (bossIntervalDecreasePerWave * bossWaveCounter)
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
            int totalWeight = bossPools.Sum(entry => entry.weight);
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
            return bossPools[0].poolID;
        }

        PoolID GetRandomMobPool()
        {
            int totalWeight = minionPools.Sum(entry => entry.weight);
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
            return minionPools[0].poolID;
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
            spawnPoint += (Vector3)randomDir * 3f;
            spawnPoint.z = 0;
            return spawnPoint;
        }
    }
}