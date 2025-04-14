using UnityEngine;
using System.Collections.Generic;
using GoodVillageGames.Game.Core.Pooling;
using System;
using static GoodVillageGames.Game.Enums.Enums;
using TriInspector;

namespace GoodVillageGames.Game.Core.Manager
{
    public class PoolManager : MonoBehaviour
    {
        public static PoolManager Instance { get; private set; }

        [Title("Pool Setup")]
        [SerializeField] private List<PoolEntry> poolEntries = new();

        private readonly Dictionary<PoolID, ObjectPool> pools = new();

        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;

            // Creating an ObjectPool for each PoolEntry
            foreach (var entry in poolEntries)
            {
                if (entry.prefab == null || entry.poolId == PoolID.None)
                {
                    Debug.LogWarning("PoolEntry is missing a prefab or poolId. Skipping entry.");
                    continue;
                }

                int poolSize = entry.config != null ? entry.config.InitialPoolSize : 20;
                bool autoExpand = entry.config != null ? entry.config.AutoExpand : true;
                int expandAmount = entry.config != null ? entry.config.ExpandAmount : 5;

                ObjectPool pool = new(entry.poolId, entry.prefab, poolSize, entry.poolContainer.transform, autoExpand, expandAmount);
                pools.Add(entry.poolId, pool);
            }
        }

        public GameObject GetPooledObject(PoolID poolId)
        {
            if (pools.TryGetValue(poolId, out ObjectPool pool))
            {
                return pool.GetGameObject();
            }
            else
            {
                Debug.LogWarning("Pool with ID '" + poolId + "' not found.");
                return null;
            }
        }

        public void ReturnPooledObject(PoolID poolId, GameObject obj)
        {
            if (pools.TryGetValue(poolId, out ObjectPool pool))
            {
                pool.ReturnGameObject(obj);
            }
            else
            {
                Debug.LogWarning("Pool with ID '" + poolId + "' not found. Destroying object.");
                Destroy(obj);
            }
        }
    }
}