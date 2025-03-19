using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;


namespace GoodVillageGames.Game.General.UI
{
    public class PlanetButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {

        private UIOutline _outline;
        private RectTransform _rectTransform;

        private readonly Vector2 _defaultPlanetSize = new(300, 300);
        private readonly Vector2 _hoverPlanetSize = new(400, 400);

        void Start()
        {
            _rectTransform = GetComponent<RectTransform>();
            _outline = GetComponentInChildren<UIOutline>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            DoIncreaseSize();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            DoDefaultSize();
        }

        private void DoIncreaseSize()
        {
            _outline.enabled = true;
            _rectTransform.DOSizeDelta(_hoverPlanetSize, 0.4f);
        }

        private void DoDefaultSize()
        {
            _rectTransform.DOSizeDelta(_defaultPlanetSize, 0.4f);
            _outline.enabled = false;
        }
    }
}
