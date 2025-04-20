using UnityEngine;
using System.Collections.Generic;
using GoodVillageGames.Game.General.UI;
using GoodVillageGames.Game.Core.Global;
using GoodVillageGames.Game.Core.Manager;
using GoodVillageGames.Game.Core.Attributes.Modifiers;

namespace GoodVillageGames.Game.Handlers.UI
{
    public class SceneUpgradeHandler : MonoBehaviour 
    { 
        [SerializeField] private List<CardUIUpdater> cardsList;
        [SerializeField] private UpgradeStatPool upgradePool;
        [SerializeField] private GameObject selectButton;
        private UpgradeStatModifier currentUpgradeSelected;

        void OnEnable()
        {
            foreach (CardUIUpdater updater in cardsList)
            {
                updater.ClearExistingVFX();
                Card card = updater.gameObject.GetComponent<Card>();
                card.OnCardClicked += SetSelectedCard;
                card.UnsetSelectedCard();
                updater.Upgrade = null;
            }

            UpdateCardsListVisuals();
            selectButton.SetActive(false);

        }

        void SetSelectedCard(Card card, UpgradeStatModifier upgrade)
        {
            currentUpgradeSelected = upgrade;
            GameObject cardGameObject = card.gameObject;
            selectButton.SetActive(true);

            foreach (CardUIUpdater updater in cardsList)
            {
                if (updater.gameObject.GetInstanceID() != cardGameObject.GetInstanceID())
                {
                    updater.gameObject.GetComponent<Card>().UnsetSelectedCard();
                }
                else
                {
                    updater.gameObject.GetComponent<Card>().SetSelectedCard();
                }
            }
        }

        private void UpdateCardsListVisuals()
        {
            if (cardsList == null || cardsList.Count == 0)
            {
                Debug.LogError("Cards list is empty or not assigned.");
                return;
            }

            foreach (CardUIUpdater card in cardsList)
            {
                UpgradeStatModifier upgrade = PickRandomUpgrade();
                if (upgrade == null)
                {
                    Debug.LogError("Could not get an upgrade for card.");
                    continue;
                }

                card.Upgrade = upgrade;
                card.SetUpgradeInfo(upgrade.Image, upgrade.Rarity);
            }
        }

        private UpgradeStatModifier PickRandomUpgrade()
        {
            if (upgradePool == null)
            {
                Debug.LogError("UpgradeStatPool is not assigned.");
                return null;
            }

            List<(float weight, List<UpgradeStatModifier> upgrades)> rarities = new()
            {
                (upgradePool.CommonWeight, upgradePool.CommonUpgrades),
                (upgradePool.UncommonWeight, upgradePool.UncommonUpgrades),
                (upgradePool.RareWeight, upgradePool.RareUpgrades),
                (upgradePool.EpicWeight, upgradePool.EpicUpgrades),
                (upgradePool.LegendaryWeight, upgradePool.LegendaryUpgrades)
            };

            float totalWeight = CalculateTotalWeight(rarities);
            if (totalWeight <= 0)
            {
                Debug.LogError("Total weight is non-positive.");
                return null;
            }

            float randomValue = Random.Range(0f, totalWeight);
            float accumulatedWeight = 0f;

            foreach (var (weight, upgrades) in rarities)
            {
                accumulatedWeight += weight;
                if (randomValue <= accumulatedWeight)
                {
                    if (upgrades == null || upgrades.Count == 0)
                    {
                        Debug.LogError("No upgrades in selected rarity.");
                        return null;
                    }

                    return upgrades[Random.Range(0, upgrades.Count)];
                }
            }

            Debug.LogError("Failed to pick a rarity.");
            return null;
        }

        private float CalculateTotalWeight(List<(float weight, List<UpgradeStatModifier> upgrades)> rarities)
        {
            float total = 0f;
            foreach (var (weight, _) in rarities)
                total += weight;
            return total;
        }

        public void OnChoiceSelected()
        {
            if (currentUpgradeSelected == null)
            {
                Debug.LogError("No upgrade selected.");
                return;
            }

            PlayerUpgraderManager.Instance.AddUpgradeToPlayer(currentUpgradeSelected);
            currentUpgradeSelected = null;
            GlobalEventsManager.Instance.ChangeGameState(Enums.Enums.GameState.GameContinue);
        }

        public void SetCurrentUpgrade(UpgradeStatModifier upgrade)
        {
            currentUpgradeSelected = upgrade;
        }
    }
}