using UnityEngine;
using System.Reflection;
using System.Collections.Generic;
using GoodVillageGames.Game.Handlers;
using GoodVillageGames.Game.Core.Global;
using GoodVillageGames.Game.Core.Attributes;
using GoodVillageGames.Game.Core.Attributes.Modifiers;

namespace GoodVillageGames.Game.Core.Manager
{
    public class PlayerUpgraderManager : MonoBehaviour
    {
        public static PlayerUpgraderManager Instance { get; private set; }

        [SerializeField] private PlayerActions player;
        private List<UpgradeStatModifier> playerUpgrades = new();
        private HealthHandler playerHealthHandler;

        void Awake()
        {
            // Singleton
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        void OnEnable() => playerHealthHandler = transform.root.GetComponentInChildren<HealthHandler>();

        public void AddUpgradeToPlayer(UpgradeStatModifier upgrade)
        {
            upgrade.UpgradeLogic.ApplyUpgrade(player);
            playerUpgrades.Add(upgrade);
            GlobalEventsManager.Instance.CollectUpgradeData(upgrade);

            if (upgrade.UpgradeLogic.StatType == Enums.Stats.StatType.MaxHealth)
                OnMaxHealthUpgrade(upgrade.UpgradeLogic.Value);
        }

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

        void OnMaxHealthUpgrade(float amount) => playerHealthHandler.CurrentHealth += amount;
    }
}