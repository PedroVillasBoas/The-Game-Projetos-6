using TMPro;
using UnityEngine;
using System.Collections.Generic;
using GoodVillageGames.Game.Core.Manager;

namespace GoodVillageGames.Game.General.UI
{
    public class StatsUI : MonoBehaviour 
    { 
        public static StatsUI Instance;

        private Dictionary<string, float> playerStats = new();
        private Dictionary<string, TextMeshProUGUI> uiElements;

        [SerializeField] private List<string> statsNames;
        [SerializeField] private List<TextMeshProUGUI> statsValuesTMPs;

        void Awake()
        {
            // Singleton
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
            
            AssembleElementsDictionary();
        }

        void OnEnable() => UpdateElementsValues();

        void UpdatePlayerStatsValues()
        {
            playerStats = PlayerUpgraderManager.Instance.GetPlayerStats();
        }

        void UpdateElementsValues()
        {
            UpdatePlayerStatsValues();

            foreach (var element in uiElements)
            {
                // MaxDefense stat -> I'm not using this one, but removing it now would be troublesome... And maybe I can use it in the future. (:
                if (element.Key == "MaxDefense") continue;

                // It exists in Player Stats? 
                if (playerStats.TryGetValue(element.Key, out float value))
                {
                    // EXP does have a stat, but stats are used for modifiers, since EXP does not have mods, we get from the EXP Manager
                    if (element.Key == "Experience")
                        element.Value.text = $"{PlayerExpManager.Instance.CurrentExp}/{PlayerExpManager.Instance.ExpToNextLevel}";
                    else
                        element.Value.text = $"{value}";
                }
                else
                {
                    Debug.LogWarning($"Stat {element.Key} not found in player stats!");
                }
            }
        }

        void AssembleElementsDictionary()
        {
            uiElements = new Dictionary<string, TextMeshProUGUI>();

            for (int i = 0; i < statsNames.Count && i < statsValuesTMPs.Count; i++)
            {
                uiElements.Add(statsNames[i], statsValuesTMPs[i]);
            }

            // Log error if lists don't match
            if (statsNames.Count != statsValuesTMPs.Count)
            {
                Debug.LogError("Stats names and TMP components lists length mismatch!");
            }
        }
    }
}