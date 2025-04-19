using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using GoodVillageGames.Game.Core.Attributes.Modifiers;
using static GoodVillageGames.Game.Enums.Enums;

namespace GoodVillageGames.Game.General.UI
{
    public class CardUIUpdater : MonoBehaviour 
    { 
        [SerializeField] private Image cardImage;
        [SerializeField] private GameObject tiltHandler;
        [SerializeField] private List<GameObject> raritiesPrefabs;
        
        private UpgradeStatModifier upgrade;
        private GameObject currentVFXInstance;

        public UpgradeStatModifier Upgrade { get => upgrade; set => upgrade = value; }

        public void SetUpgradeInfo(Sprite image, UpgradeRarity upgradeRarity)
        {
            cardImage.sprite = image;
            var vfxPrefab = GetRarityVFX(upgradeRarity);
            AddRarityVFX(vfxPrefab);
        }

        GameObject GetRarityVFX(UpgradeRarity rarity)
        {
            // Search by name containing the rarity string
            string targetName = $"Rarity - Frame - Image - {rarity}";
            foreach (var prefab in raritiesPrefabs)
            {
                if (prefab != null && prefab.name.StartsWith(targetName))
                    return prefab;
            }

            Debug.LogError($"Missing VFX prefab for rarity: {rarity}");
            return null;
        }

        void AddRarityVFX(GameObject prefab)
        {
            ClearExistingVFX();

            if (!prefab || !tiltHandler)
            {
                Debug.LogError("Missing components for VFX instantiation");
                return;
            }

            try {
                // Reference to the new prefab just instantiated
                currentVFXInstance = Instantiate(prefab, tiltHandler.transform);
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to instantiate VFX: {e.Message}");
            }
        }

        public void ClearExistingVFX()
        {
            if (currentVFXInstance)
            {
                Destroy(currentVFXInstance);
                currentVFXInstance = null;
            }
        }
    }
}