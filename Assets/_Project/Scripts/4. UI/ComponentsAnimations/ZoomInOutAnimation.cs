using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using GoodVillageGames.Game.Interfaces;
using static GoodVillageGames.Game.Enums.Enums;
using GoodVillageGames.Game.Core.Manager;

namespace GoodVillageGames.Game.General.UI.Animations
{
    public class ZoomInOutAnimation : MonoBehaviour, IComponentAnimation
    {
        [SerializeField] private Vector2 _sizeAfterZoom;
        [SerializeField] private float _duration;
        [SerializeField] private Tweener _componentTweener;
        [SerializeField] private AnimationID _animationID;
        [SerializeField] private UIAnimationType _UIAnimationType;

        private RectTransform _componentRectTransform;
        private Vector2 _originalSize;

        public Tweener ComponentTween { get => _componentTweener; set => _componentTweener = value; }
        public float Duration { get => _duration; set => _duration = value; }
        public AnimationID AnimationID { get => _animationID; set => _animationID = value; }
        public UIAnimationType UIAnimationType { get => _UIAnimationType; set => _UIAnimationType = value; }

        void Awake()
        {
            _componentRectTransform = GetComponent<Image>().rectTransform;
            _originalSize = _componentRectTransform.sizeDelta;

            BuildAnimation();
            AddComponentToSceneManagerStack();

        }

        void Zoom()
        {
            _componentTweener = _componentRectTransform.DOSizeDelta(_sizeAfterZoom, _duration);
            _componentTweener.Pause();
        }

        public void BuildAnimation()
        {
            Zoom();

        }

        public void AddComponentToSceneManagerStack()
        {
            EventsManager.Instance.AddComponentToStackTriggered(gameObject);
        }

    }
}
