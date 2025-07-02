using UnityEngine;
using System.Collections.Generic;

namespace GoodVillageGames.Game.Core.Manager
{
    public class MobSpawnAdvisor : MonoBehaviour
    {
        public static MobSpawnAdvisor Instance { get; private set; }

        [SerializeField] private List<GameObject> mobsPoolContainers;

        [SerializeField] private int maxActiveMobAmount = 100;

        private int totalActiveMobs = 0;
        public int ActiveMobCount => totalActiveMobs;

        void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        void Update()
        {
            Debug.Log($"Total mobs: {totalActiveMobs}");
        }

        public void IncrementActiveMobCount() => totalActiveMobs++;
        public void DecrementActiveMobCount() => totalActiveMobs--;

        public bool CheckCanSpawn(int amountToSpawn)
        {
            return (amountToSpawn + totalActiveMobs) <= maxActiveMobAmount;
        }

        public int GetAllowedSpawnCount()
        {
            int availableSlots = maxActiveMobAmount - totalActiveMobs;
            return Mathf.Max(0, availableSlots);
        }
    }
}