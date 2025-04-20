using UnityEngine;
using GoodVillageGames.Game.Core.Attributes.Modifiers;
using GoodVillageGames.Game.Core.Attributes;

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

        public void CreatePopup(UpgradeStatModifier upgrade, Vector2 screenPosition, Vector2 objectSize)
        {
            CreatePopupInternal(
                upgrade.Name,
                upgrade.Rarity.ToString(),
                upgrade.Description,
                screenPosition,
                objectSize
            );
        }

        public void CreatePopup(StatUIElementInfo statInfo, Vector2 screenPosition, Vector2 objectSize)
        {
            CreatePopupInternal(
                statInfo.Name,
                "Stat",
                statInfo.Description,
                screenPosition,
                objectSize
            );
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