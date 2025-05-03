using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using GoodVillageGames.Game.Core.ScriptableObjects;
using System.Collections;
using GoodVillageGames.Game.Core.Global;


namespace GoodVillageGames.Game.General.UI
{
    public class PlanetButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [SerializeField] private DifficultyInfo difficultyInfo;
        [SerializeField] private TextMeshProUGUI title; 
        [SerializeField] private TextMeshProUGUI description;
        [SerializeField] private RectTransform planetRectTransform;

        private RectTransform _rectTransform;
        private UIOutline _outline;
        private Coroutine _sizeCoroutine;

        private readonly Vector2 _defaultPlanetSize = new(300, 300);
        private readonly Vector2 _hoverPlanetSize = new(400, 400);
        private const float ANIMATION_DURATION = 0.4f;


        void Start()
        {
            _rectTransform = GetComponent<RectTransform>();
            _outline = GetComponentInChildren<UIOutline>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            StartSizeAnimation(_hoverPlanetSize);
            UpdateUI();
            _outline.enabled = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            StartSizeAnimation(_defaultPlanetSize);
            _outline.enabled = false;
        }

        public void OnPointerClick(PointerEventData eventData) => GlobalEventsManager.Instance.GameDifficultyTriggerEvent(difficultyInfo.gameDifficulty);

        void StartSizeAnimation(Vector2 targetSize)
        {
            if (_sizeCoroutine != null)
            {
                StopCoroutine(_sizeCoroutine);
            }
            _sizeCoroutine = StartCoroutine(AnimateSize(targetSize));
        }

        IEnumerator AnimateSize(Vector2 targetSize)
        {
            Vector2 startSize = _rectTransform.sizeDelta;
            Vector2 planetStartSize = planetRectTransform.sizeDelta;
            float elapsed = 0f;

            while (elapsed < ANIMATION_DURATION)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / ANIMATION_DURATION);
                _rectTransform.sizeDelta = Vector2.Lerp(startSize, targetSize, t);
                planetRectTransform.sizeDelta = Vector2.Lerp(planetStartSize, targetSize, t);
                yield return null;
            }

            _rectTransform.sizeDelta = targetSize;
            planetRectTransform.sizeDelta = targetSize;
            _sizeCoroutine = null;
        }

        void UpdateUI()
        {
            title.text = difficultyInfo.Name;
            description.text = difficultyInfo.Description;
        }
    }
}
