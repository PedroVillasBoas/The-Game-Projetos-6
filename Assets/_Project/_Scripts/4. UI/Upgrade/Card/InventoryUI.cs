using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using GoodVillageGames.Game.Core.Manager;
using GoodVillageGames.Game.Core.Attributes.Modifiers;

namespace GoodVillageGames.Game.General.UI
{
    public class InventoryUI : MonoBehaviour 
    { 
        [SerializeField] private GameObject gridContainer;
        [SerializeField] private List<GameObject> upgradeElementPrefabs;

        private List<UIUpgradeElement> uiUpgradesList = new();
        private List<UpgradeStatModifier> currentDisplayedUpgrades = new();

        void OnEnable()
        {
            UpdateInventoryUI();
        }

        void UpdateInventoryUI()
        {
            var newUpgradesList = PlayerUpgraderManager.Instance.GetPlayerUpgrades();
            
            // Differences using index-based comparison
            var (additions, removals) = DiffUpgradeLists(newUpgradesList);
            
            HandleRemovals(removals);
            HandleAdditions(newUpgradesList, additions);
        }

        (List<UpgradeStatModifier> additions, List<int> removals) DiffUpgradeLists(List<UpgradeStatModifier> newList)
        {
            var additions = new List<UpgradeStatModifier>();
            var removals = new List<int>();

            // Removals
            for (int i = currentDisplayedUpgrades.Count - 1; i >= 0; i--)
            {
                if (!newList.Contains(currentDisplayedUpgrades[i]))
                {
                    removals.Add(i);
                }
            }

            // Additions
            foreach (var upgrade in newList)
            {
                int currentCount = currentDisplayedUpgrades.Count(u => u == upgrade);
                int newCount = newList.Count(u => u == upgrade);
                
                if (newCount > currentCount)
                {
                    additions.Add(upgrade);
                }
            }

            return (additions, removals);
        }

        void HandleRemovals(List<int> removalIndices)
        {
            foreach (var index in removalIndices.OrderByDescending(i => i))
            {
                if (index < uiUpgradesList.Count)
                {
                    Destroy(uiUpgradesList[index].gameObject);
                    uiUpgradesList.RemoveAt(index);
                    currentDisplayedUpgrades.RemoveAt(index);
                }
            }
        }

        void HandleAdditions(List<UpgradeStatModifier> fullList, List<UpgradeStatModifier> additions)
        {
            foreach (var upgrade in additions)
            {
                var prefab = upgradeElementPrefabs.FirstOrDefault(p => 
                    p.name == upgrade.UpgradeLogic.StatType.ToString());

                if (prefab != null)
                {
                    var newElement = CreateUpgradeElement(prefab, upgrade);
                    // Placing at the CORRECT position to maintain the order that the player got the upgrade
                    int targetIndex = fullList.LastIndexOf(upgrade);
                    uiUpgradesList.Insert(targetIndex, newElement);
                    currentDisplayedUpgrades.Insert(targetIndex, upgrade);
                }
            }
        }

        UIUpgradeElement CreateUpgradeElement(GameObject prefab, UpgradeStatModifier modifier)
        {
            var newElement = Instantiate(prefab, gridContainer.transform);
            newElement.SetActive(true);

            // Getting and Setting the UIUpgradeElement component
            if(!newElement.TryGetComponent<UIUpgradeElement>(out var uiElement))
            {
                Debug.LogError("Prefab missing UIUpgradeElement component!");
                Destroy(newElement);
                return null;
            }

            // Upgrade Reference
            uiElement.Upgrade = modifier;
            return uiElement;
        }
    }
}