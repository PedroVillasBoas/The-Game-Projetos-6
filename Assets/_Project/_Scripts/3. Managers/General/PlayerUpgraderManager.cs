using UnityEngine;
using System.Reflection;
using System.Collections.Generic;
using GoodVillageGames.Game.Core.Attributes;
using GoodVillageGames.Game.Core.Attributes.Modifiers;
using GoodVillageGames.Game.Core.Global;

namespace GoodVillageGames.Game.Core.Manager
{
    public class PlayerUpgraderManager : MonoBehaviour
    {
        public static PlayerUpgraderManager Instance { get; private set; }

        [SerializeField] private PlayerActions player;
        private List<UpgradeStatModifier> playerUpgrades = new();

        void Awake()
        {
            // Singleton
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        public void AddUpgradeToPlayer(UpgradeStatModifier upgrade)
        {
            upgrade.UpgradeLogic.ApplyUpgrade(player);
            playerUpgrades.Add(upgrade);
            GlobalEventsManager.Instance.CollectUpgradeData(upgrade);
        }

        // not using yet
        public int GetPlayerCurrentLevel()
        {
            return PlayerExpManager.Instance.CurrentLevel;
        }

        public List<UpgradeStatModifier> GetPlayerUpgrades()
        {
            return playerUpgrades;
        }

        public Dictionary<string, float> GetPlayerStats()
        {
            Dictionary<string, float> statsDict = new();
            PropertyInfo[] properties = typeof(Stats).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo property in properties)
            {
                if (property.PropertyType == typeof(float))
                {
                    float value = (float)property.GetValue(player.Stats);
                    statsDict.Add(property.Name, value);
                }
            }

            return statsDict;
        }
    }
}