using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using GoodVillageGames.Game.Interfaces;
using static GoodVillageGames.Game.Enums.Enums;
using GoodVillageGames.Game.Core.Manager;

namespace GoodVillageGames.Game.General.UI.Animations
{
    public class FadeInOutAnimation : MonoBehaviour, IComponentAnimation
    {
        [SerializeField] private bool _fadeIN = false;
        [SerializeField] private float _duration;
        [SerializeField] private AnimationID _animationID;
        [SerializeField] private UIAnimationType _UIAnimationType;


        private Image _image;
        private Tweener _componentTweener;

        public float Duration { get => _duration; set => _duration = value; }
        public Tweener ComponentTween { get => _componentTweener; set => _componentTweener = value; }
        public AnimationID AnimationID { get => _animationID; set => _animationID = value; }
        public UIAnimationType UIAnimationType { get => _UIAnimationType; set => _UIAnimationType = value; }

        void Awake()
        {
            _image = GetComponent<Image>();
            BuildAnimation();
            AddComponentToSceneManagerStack();
        }
        void FadeIN()
        {
            _componentTweener = _image.DOFade(1f, _duration);
            _componentTweener.Pause();
        }
        void FadeOUT()
        {
            _componentTweener = _image.DOFade(0f, _duration);
            _componentTweener.Pause();
        }

        public void BuildAnimation()
        {
            if (_fadeIN)
                FadeIN();
            else
                FadeOUT();
        }

        public void AddComponentToSceneManagerStack()
        {
            EventsManager.Instance.AddComponentToStackTriggered(gameObject);
        }
    }
}
