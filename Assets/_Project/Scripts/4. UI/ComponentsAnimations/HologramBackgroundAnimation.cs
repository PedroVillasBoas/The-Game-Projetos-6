using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using GoodVillageGames.Game.Interfaces;
using GoodVillageGames.Game.Core.Manager;
using static GoodVillageGames.Game.Enums.Enums;
using UnityEngine.AI;

namespace GoodVillageGames.Game.General.UI.Animations
{
    public class HologramBackgroundAnimation : MonoBehaviour, IComponentAnimation
    {
        [SerializeField] private float _duration;
        [SerializeField] private AnimationID _animationID;
        [SerializeField] private UIAnimationType _UIAnimationType;

        private Sequence _componentTweener = DOTween.Sequence();
        private Vector2 _finalSize;
        private Vector2 _originalSize = new(0f, 0f);
        private RectTransform _componentRectTransform;
        private Image _componentImage;

        public Tween ComponentTween { get => _componentTweener; set => _componentTweener = (Sequence)value; }
        public float Duration { get => _duration; set => _duration = value; }
        public AnimationID AnimationID { get => _animationID; set => _animationID = value; }
        public UIAnimationType UIAnimationType { get => _UIAnimationType; set => _UIAnimationType = value; }

        void Awake()
        {
            _componentImage = GetComponent<Image>();
            _componentRectTransform = _componentImage.rectTransform;
            _finalSize = _componentRectTransform.sizeDelta;

            BuildAnimation();
            AddComponentToSceneManagerStack();
        }

        void OnEnable()
        {
            _componentRectTransform.sizeDelta = _originalSize;
        }

        public void BuildAnimation()
        {
            _componentTweener.Append(_componentImage.DOFade(0, _duration).SetLoops(10, LoopType.Yoyo));
            _componentTweener.Insert(0, _componentRectTransform.DOSizeDelta(new(_componentRectTransform.sizeDelta.x, _finalSize.y), _duration));
            _componentTweener.Join(_componentRectTransform.DOSizeDelta(new(_finalSize.x, _componentRectTransform.sizeDelta.y), _duration))
            .SetEase(Ease.InExpo).OnComplete(EnableChildren);
        }

        public void EnableChildren()
        {
            foreach (Transform child in gameObject.transform)
            {
                child.gameObject.SetActive(true);
            }
        }

        public void AddComponentToSceneManagerStack()
        {
            EventsManager.Instance.AddComponentToStackTriggered(gameObject);
        }
    }
}
