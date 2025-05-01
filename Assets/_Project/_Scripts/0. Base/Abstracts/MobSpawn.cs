using UnityEngine;
using System.Linq;
using TriInspector;
using System.Collections;
using System.Collections.Generic;
using GoodVillageGames.Game.Enums;
using GoodVillageGames.Game.Core.Global;
using GoodVillageGames.Game.Core.Manager;
using GoodVillageGames.Game.Enums.Pooling;

namespace GoodVillageGames.Game.Core.MobSpawning
{
    [DeclareFoldoutGroup("Spawn"), DeclareFoldoutGroup("Geral")]
    public abstract class MobSpawn : MonoBehaviour
    {
        [Title("General Config")]
        [SerializeField, Group("Geral")] protected Transform playerTransform;
        [SerializeField, Group("Geral")] protected Camera mainCamera;


        [Title("Spawning Config")]
        [SerializeField, Group("Spawn")] protected bool hasSpawnDelay = false;
        [SerializeField, Group("Spawn")] protected int baseMobAmount = 2;
        [SerializeField, Group("Spawn")] protected int baseMobAdditivePerWave = 1;
        [SerializeField, Group("Spawn")] protected float spawnAmountMult = 0.05f;
        [SerializeField, Group("Spawn")] protected float minTimeBetweenWaves = 0.2f;
        [SerializeField, Group("Spawn")] protected float minSpawnDistance = 2f;
        [ShowIf("hasSpawnDelay", true), SerializeField, Group("Spawn")] protected float initialSpawnDelay = 2f;
        [SerializeField, Group("Spawn")] protected List<MobPoolEntry> mobPools = new();

        // Local variables
        private int waveCounter;
        private List<Vector3> currentWaveSpawnPositions = new();

        // Mob pool config
        [System.Serializable]
        public struct MobPoolEntry
        {
            public PoolID poolID;
            [Min(1)] public int weight;
        }

        protected abstract void BeginSpawnCoroutine();
        protected abstract IEnumerator SpawnRoutine();
        protected abstract void SpawnWave(int mobAmountToSpawn);
        protected abstract PoolID GetRandomMobPool();

        protected void OnEnable() => GlobalEventsManager.Instance.ChangeGameStateEventTriggered += OnGameStateChanged;
        protected void OnDestroy() => GlobalEventsManager.Instance.ChangeGameStateEventTriggered -= OnGameStateChanged;

        protected void OnGameStateChanged(GameState gameState)
        {
            if (gameState == GameState.GameBegin)
                BeginSpawnCoroutine();
        }

        protected int CalculateMobsPerWave()
        {
            return baseMobAmount + Mathf.FloorToInt(baseMobAdditivePerWave * waveCounter);
        }

        protected float CalculateSpawnInterval()
        {
            float time = SceneTimerManager.Instance.GetRunTime();
            return Mathf.Max(minTimeBetweenWaves, initialSpawnDelay - (spawnAmountMult * time));
        }

        protected Vector3 GetValidSpawnPosition()
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

        protected Vector3 GetOffscreenSpawnPosition()
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