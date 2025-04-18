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
            int rarityIndex = (int)rarity;
            
            if (raritiesPrefabs == null || rarityIndex < 0 || rarityIndex >= raritiesPrefabs.Count)
            {
                Debug.LogError($"Missing VFX prefab for rarity: {rarity}");
                return null;
            }

            return raritiesPrefabs[rarityIndex];
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
                Instantiate(prefab, tiltHandler.transform);
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to instantiate VFX: {e.Message}");
            }
        }

        void ClearExistingVFX()
        {
            if (currentVFXInstance)
            {
                Destroy(currentVFXInstance);
                currentVFXInstance = null;
            }
        }
    }
}