using UnityEngine;
using GoodVillageGames.Game.Core.Attributes;
using GoodVillageGames.Game.Core.ScriptableObjects;
using GoodVillageGames.Game.Core.Attributes.Modifiers;
using UnityEngine.Localization.Settings;

namespace GoodVillageGames.Game.General.UI
{
    public class UIPopupManager : MonoBehaviour
    {
        public static UIPopupManager Instance { get; private set; }

        [SerializeField] private GameObject popupPrefab;
        [SerializeField] private Canvas popupCanvas;
        private GameObject activePopupInstance;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                popupCanvas = GetComponentInParent<Canvas>();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        bool IsLocalePortuguese()
        {
            string localeCode = LocalizationSettings.SelectedLocale.Identifier.Code;
            return localeCode.StartsWith("pt");
        }

        public void CreatePopup(UpgradeStatModifier upgrade, Vector2 screenPosition, Vector2 objectSize)
        {
            // Determina qual texto usar com base no idioma
            string name = IsLocalePortuguese() ? upgrade.PortName : upgrade.Name;
            string description = IsLocalePortuguese() ? upgrade.PortDescription : upgrade.Description;

            CreatePopupInternal(name, upgrade.Rarity.ToString(), description, screenPosition, objectSize);
        }

        public void CreatePopup(RarityInfo rarityInfo, Vector2 screenPosition, Vector2 objectSize)
        {
            bool isPortuguese = IsLocalePortuguese();
            string name = isPortuguese ? rarityInfo.PortName : rarityInfo.Name;
            string type = isPortuguese ? "Raridade" : "Rarity";
            string description = isPortuguese ? rarityInfo.PortDescription : rarityInfo.Description;

            CreatePopupInternal(name, type, description, screenPosition, objectSize);
        }

        public void CreatePopup(StatUIElementInfo statInfo, Vector2 screenPosition, Vector2 objectSize)
        {
            bool isPortuguese = IsLocalePortuguese();
            string name = isPortuguese ? statInfo.PortName : statInfo.Name;
            string description = isPortuguese ? statInfo.PortDescription : statInfo.Description;

            CreatePopupInternal(name, "Stat", description, screenPosition, objectSize);
        }

        private void CreatePopupInternal(string name, string type, string description, Vector2 screenPosition, Vector2 objectSize)
        {
            if (popupPrefab == null)
            {
                Debug.LogError("Popup prefab is not assigned in UIPopupManager.");
                return;
            }

            // Destroy existing popup if something went wrong
            if (activePopupInstance != null)
            {
                Destroy(activePopupInstance);
            }

            RectTransform canvasRect = popupCanvas.GetComponent<RectTransform>();
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvasRect,
                screenPosition,
                popupCanvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : popupCanvas.worldCamera,
                out Vector2 localPosition))
            {
                Debug.LogError("Failed to convert screen position to canvas position.");
                return;
            }

            activePopupInstance = Instantiate(popupPrefab, popupCanvas.transform);
            activePopupInstance.SetActive(true);

            RectTransform popupRect = activePopupInstance.GetComponent<RectTransform>();

            float elementX = localPosition.x;
            float elementWidth = objectSize.x;
            float popupWidth = popupRect.rect.width;

            float newX;
            if (elementX < 0) // Left side of canvas
            {
                // Position to the right of the element that is asking for the popup
                newX = localPosition.x + (elementWidth + popupWidth) * 0.6f;
            }
            else if (elementX > 0) // Right side of canvas
            {
                // Position to the left of the element that is asking for the popup
                newX = localPosition.x - (elementWidth + popupWidth) * 0.6f;
            }
            else // Center of canvas
            {
                // Default to right side
                newX = localPosition.x + (elementWidth + popupWidth) * 0.6f;
            }

            popupRect.anchoredPosition = new Vector2(newX, localPosition.y);

            // Update popup content
            if (activePopupInstance.TryGetComponent<PopupUpdater>(out var popupUpdater))
            {
                popupUpdater.SetPopupInfo(
                    name,
                    type,
                    description
                );
            }
            else
            {
                Debug.LogError("PopupUpdater component not found on popupPrefab.");
            }
        }

        public void DestroyPopup()
        {
            if (activePopupInstance != null) Destroy(activePopupInstance);
        }
    }
}