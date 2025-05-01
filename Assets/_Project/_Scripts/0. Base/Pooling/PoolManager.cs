using UnityEngine;
using TriInspector;
using System.Collections.Generic;
using GoodVillageGames.Game.Core.Pooling;
using GoodVillageGames.Game.Enums.Pooling;

namespace GoodVillageGames.Game.Core.Manager
{
    public class PoolManager : MonoBehaviour
    {
        public static PoolManager Instance { get; private set; }

        [Title("Pools Setup")]
        [SerializeField] private List<PoolEntry> playerPoolEntries = new();
        [SerializeField] private List<PoolEntry> minionsPoolEntries = new();
        [SerializeField] private List<PoolEntry> bossesPoolEntries = new();
        [SerializeField] private List<PoolEntry> projectilesPoolEntries = new();
        [SerializeField] private List<PoolEntry> expPoolEntries = new();
        [SerializeField] private List<PoolEntry> itemPoolEntries = new();
        [SerializeField] private List<PoolEntry> dmgNumbersPoolEntries = new();

        private readonly Dictionary<PoolID, ObjectPool> poolsDict = new();

        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            InitializePools();
        }

        // Creating an ObjectPool for each PoolEntry
        void InitializePools()
        {
            var allPoolEntries = GetAllPoolEntries();
            
            foreach (var entry in allPoolEntries)
            {
                if (!IsPoolEntryValid(entry))
                {
                    Debug.LogWarning($"Invalid PoolEntry: {(entry.prefab == null ? "Missing prefab" : "Missing PoolID")}");
                    continue;
                }

                CreatePool(entry);
            }
        }

        IEnumerable<PoolEntry> GetAllPoolEntries()
        {
            var allEntries = new List<PoolEntry>();

            allEntries.AddRange(playerPoolEntries);
            allEntries.AddRange(minionsPoolEntries);
            allEntries.AddRange(bossesPoolEntries);
            allEntries.AddRange(expPoolEntries);
            allEntries.AddRange(projectilesPoolEntries);
            allEntries.AddRange(itemPoolEntries);
            allEntries.AddRange(dmgNumbersPoolEntries);
            
            return allEntries;
        }

        bool IsPoolEntryValid(PoolEntry entry)
        {
            return entry.prefab != null && entry.poolId != PoolID.None;
        }

        void CreatePool(PoolEntry entry)
        {
            var config = entry.config;
            var poolSize = config?.InitialPoolSize ?? 20;
            var autoExpand = config?.AutoExpand ?? true;
            var expandAmount = config?.ExpandAmount ?? 5;

            var pool = new ObjectPool(
                entry.poolId,
                entry.prefab,
                poolSize,
                entry.poolContainer.transform,
                autoExpand,
                expandAmount
            );

            poolsDict.Add(entry.poolId, pool);
        }

        public GameObject GetPooledObject(PoolID poolId)
        {
            if (!poolsDict.TryGetValue(poolId, out var pool))
            {
                Debug.LogWarning($"Pool with ID '{poolId}' not found.");
                return null;
            }

            return pool.GetGameObject();
        }

        public void ReturnPooledObject(PoolID poolId, GameObject obj)
        {
            if (obj == null) return;

            if (poolsDict.TryGetValue(poolId, out var pool))
            {
                pool.ReturnGameObject(obj);
            }
            else
            {
                Debug.LogWarning($"Pool with ID '{poolId}' not found. Destroying object.");
                Destroy(obj);
            }
        }
    }
}