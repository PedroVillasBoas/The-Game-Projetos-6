using System.Collections;
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

        [Title("Circle Spawn")]
        [SerializeField, Group("Special Spawn")] private float circleSpawnCooldown = 10f;
        [SerializeField, Group("Special Spawn")] private int circleMobCount = 20;
        [SerializeField, Group("Special Spawn")] private float circleRadius = 50f;

        // [Title("Boss Spawning")]
        // [SerializeField, Group("Boss")] private float initialBossSpawnInterval = 180f;
        // [SerializeField, Group("Boss")] private float minBossSpawnInterval = 120f;
        // [SerializeField, Group("Boss")] private float spawnBossRateIncrease = 0.2f;
        // [SerializeField, Group("Boss")] private int baseBossPerWave = 1;
        // [SerializeField, Group("Boss")] private float bossIncreasePerCycle = 1f;

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
            GlobalEventsManager.Instance.StartRunEventTriggered += BeginSpawnCoroutines;
        }

        void OnDestroy()
        {
            GlobalEventsManager.Instance.StartRunEventTriggered -= BeginSpawnCoroutines;
        }

        void Start()
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
            mainCamera = Camera.main;
        }

        void BeginSpawnCoroutines()
        {
            StartCoroutine(RegularSpawnRoutine());
            StartCoroutine(SpecialSpawnCheckRoutine());
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
                if (Time.time - lastCircleSpawnTime > circleSpawnCooldown &&
                    Random.value < 0.3f) // 30% chance check
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
            spawnPoint += (Vector3)randomDir * 2f;
            spawnPoint.z = 0;
            return spawnPoint;
        }

    }
}
