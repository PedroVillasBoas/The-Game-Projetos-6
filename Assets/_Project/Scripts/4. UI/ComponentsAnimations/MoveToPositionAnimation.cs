using DG.Tweening;
using UnityEngine;
using GoodVillageGames.Game.Interfaces;
using GoodVillageGames.Game.Core.Manager;
using static GoodVillageGames.Game.Enums.Enums;

namespace GoodVillageGames.Game.General.UI.Animations
{
    public class MoveToPositionAnimation : MonoBehaviour, IComponentAnimation
    {
        [SerializeField] private float _duration;
        [SerializeField] private AnimationID _animationID;
        [SerializeField] private UIAnimationType _UIAnimationType;
        [SerializeField] private Vector2 _finalPosition;

        private Tweener _componentTweener;
        private RectTransform _componentRectTransform;

        public Tween ComponentTween { get => _componentTweener; set => _componentTweener = (Tweener)value; }
        public float Duration { get => _duration; set => _duration = value; }
        public AnimationID AnimationID { get => _animationID; set => _animationID = value; }
        public UIAnimationType UIAnimationType { get => _UIAnimationType; set => _UIAnimationType = value; }

        void Awake()
        {
            _componentRectTransform = GetComponent<RectTransform>();

            BuildAnimation();
            AddComponentToSceneManagerStack();
        }

        public void BuildAnimation()
        {
            _componentTweener = _componentRectTransform.DOAnchorPos(_finalPosition, _duration).SetEase(Ease.InCubic);
        }

        public void AddComponentToSceneManagerStack()
        {
            EventsManager.Instance.AddComponentToStackTriggered(gameObject);
        }
    }
}
