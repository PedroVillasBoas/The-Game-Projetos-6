using GoodVillageGames.Game.Core.Attributes;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GoodVillageGames.Game.General.UI
{
    public class UIStatElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    { 
        [SerializeField] private StatUIElementInfo info;
        [SerializeField] private RectTransform elementRect;

        void OnDisable()
        {
            UIPopupManager.Instance.DestroyPopup();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            AskPopup();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            UIPopupManager.Instance.DestroyPopup();
        }

        void AskPopup()
        {
            // Getting the element world corners
            Vector3[] corners = new Vector3[4];
            elementRect.GetWorldCorners(corners);

            // Calculating center position in world space
            Vector3 worldCenter = (corners[0] + corners[2]) * 0.5f;

            // Converting to screen space
            Canvas canvas = transform.root.GetComponent<Canvas>();
            Vector2 screenPosition = RectTransformUtility.WorldToScreenPoint(
                canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera, worldCenter
            );

            // Getting current actual size
            Vector2 currentSize = elementRect.rect.size;

            UIPopupManager.Instance.CreatePopup(
                info,
                screenPosition,
                currentSize
            );
        }
    }
}