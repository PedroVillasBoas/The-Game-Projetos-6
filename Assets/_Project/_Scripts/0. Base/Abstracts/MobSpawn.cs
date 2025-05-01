using System;
using UnityEngine;
using System.Linq;
using TriInspector;
using System.Collections;
using System.Collections.Generic;
using GoodVillageGames.Game.Enums;
using GoodVillageGames.Game.Core.Global;
using GoodVillageGames.Game.Core.Manager;
using GoodVillageGames.Game.Enums.Pooling;
using GoodVillageGames.Game.Core.ScriptableObjects;

namespace GoodVillageGames.Game.Core.MobSpawning
{
    [DeclareFoldoutGroup("Geral")]
    public abstract class MobSpawn : MonoBehaviour
    {
        #region Variables

        [Title("General Config")]
        [SerializeField, Group("Geral")] protected Transform playerTransform;
        [SerializeField, Group("Geral")] protected Camera mainCamera;
        [SerializeField, Group("Geral")] protected float spawnAreaOffset = 4f;
        [SerializeField, Group("Geral")] protected bool isSpecial;
        [Space(15), ShowIf("isSpecial", false), SerializeField] protected MobSpawnConfig mobSpawnConfig;
        [Space(15), ShowIf("isSpecial", true), SerializeField] protected SpecialMobSpawnConfig specialMobSpawnConfig;

        protected int waveCounter;
        protected List<Vector3> currentWaveSpawnPositions = new();

        // Flag
        protected bool isSpawning;

        // Coroutines
        protected Coroutine spawnCoroutine;

        // Proterties
        protected float Delay 
        {
            get { return isSpecial ? specialMobSpawnConfig.InitialSpawnDelay : mobSpawnConfig.InitialSpawnDelay; }
        }

        #endregion

        #region Class Methods

        protected void OnEnable() => GlobalEventsManager.Instance.ChangeGameStateEventTriggered += OnGameStateChanged;
        protected void OnDestroy() => GlobalEventsManager.Instance.ChangeGameStateEventTriggered -= OnGameStateChanged;

        protected void OnGameStateChanged(GameState gameState)
        {
            if (gameState == GameState.GameBegin)
                if (Delay <= 0)
                    BeginSpawnCoroutine();
                else
                    spawnCoroutine = StartCoroutine(InitialDelay(Delay));
        }

        IEnumerator InitialDelay(float delay)
        {
            while (true)
            {
                Debug.Log($"Began Initial Delay of {gameObject.name} with Delay Amount: {delay} at {Time.time}");
                yield return new WaitForSeconds(delay);
                BeginSpawnCoroutine();
                break;
            }
        }

        protected void BeginSpawnCoroutine()
        {
            if (isSpawning) return;

            // Stopping any existing coroutine
            if (spawnCoroutine != null) StopCoroutine(spawnCoroutine);

            // Starting new coroutine
            spawnCoroutine = StartCoroutine(SpawnCoroutine());

            isSpawning = true;
        }

        protected int CalculateMobsPerWave()
        {
            return mobSpawnConfig.BaseMobAmount + Mathf.CeilToInt(mobSpawnConfig.BaseMobAdditivePerWave * waveCounter);
        }

        protected float CalculateSpawnInterval()
        {
            float time = SceneTimerManager.Instance.GetRunTime();
            Debug.Log($"Calculating Spawn Interval at {time} in SceneTimerManager");
            return Mathf.Max(mobSpawnConfig.MinTimeBetweenWaves, mobSpawnConfig.InitialSpawnDelay - (mobSpawnConfig.SpawnAmountMult * time));
        }

        protected PoolID PickRandomPoolID<TEntry>(IReadOnlyList<TEntry> entries, Func<TEntry,int>  weightSelector, Func<TEntry,PoolID> idSelector)
        {
            int totalWeight = entries.Sum(weightSelector);
            if (totalWeight <= 0)
                throw new InvalidOperationException($"No positive weights in {nameof(entries)}");

            int roll = UnityEngine.Random.Range(0, totalWeight);

            int acc = 0;
            foreach (var e in entries)
            {
                acc += weightSelector(e);
                if (roll < acc)
                    return idSelector(e);
            }

            return idSelector(entries[0]);
        }

       protected Vector3 GetValidSpawnPosition()
        {
            Vector3 spawnPos;
            int attempts = 0;
            bool validPosition;

            do {
                validPosition = true;
                spawnPos = GetOffscreenSpawnPosition();

                foreach (var pos in currentWaveSpawnPositions)
                {
                    if (Vector3.Distance(spawnPos, pos) < mobSpawnConfig.MinSpawnDistance)
                    {
                        validPosition = false;
                        break;
                    }
                }

                var colliders = Physics2D.OverlapCircleAll(spawnPos, mobSpawnConfig.MinSpawnDistance);
                if (colliders.Any(c => c.CompareTag("Enemy")))
                    validPosition = false;

                attempts++;
            } while (!validPosition && attempts < 10);

            return spawnPos;
        }

        protected Vector3 GetOffscreenSpawnPosition()
        {
            Vector2 randomDir = UnityEngine.Random.insideUnitCircle.normalized;
            Vector3 spawnPoint = mainCamera.ViewportToWorldPoint(
                new Vector3(
                    randomDir.x < 0 ? 0.1f : 0.9f,
                    randomDir.y < 0 ? 0.1f : 0.9f,
                    mainCamera.nearClipPlane
                )
            );
            spawnPoint += (Vector3)randomDir * spawnAreaOffset;
            spawnPoint.z = 0;
            return spawnPoint;
        }

        #endregion

        #region Abstract Methods

        protected abstract void SpawnWave(int mobAmountToSpawn);
        protected abstract PoolID GetRandomMobPool();
        protected abstract IEnumerator SpawnCoroutine();

        #endregion
    }
}