using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections.Generic;
using GoodVillageGames.Game.Interfaces;
using static GoodVillageGames.Game.Enums.Enums;
using GoodVillageGames.Game.General.UI.Animations.Config;

namespace GoodVillageGames.Game.General.UI.Animations
{
    [RequireComponent(typeof(RectTransform), typeof(Image))]
    public class HologramAnimation : MonoBehaviour, IComponentAnimation
    {
        [SerializeField] private List<HologramAnimationConfig> _hologramConfigs = new();

        private RectTransform _rectTransform;
        private Image _image;
        private Dictionary<AnimationTransitionID, List<Tween>> _animationsDict = new();

        public Dictionary<AnimationTransitionID, List<Tween>> Animations => _animationsDict;

        void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _image = GetComponent<Image>();
            BuildAnimations();
        }

        public void BuildAnimations()
        {
            foreach (var config in _hologramConfigs)
            {
                if (config.ComponentToAnimate == null)
                {
                    Debug.LogError($"Component not defined in {gameObject.name}");
                    continue;
                }

                var sequence = DOTween.Sequence();

                // Size Animation
                sequence.Append(_rectTransform.DOSizeDelta(config.FinalSize, config.SizeChangeDuration)
                    .SetEase(Ease.InExpo));

                // Blink Effect
                if (config.Blink)
                {
                    sequence.Join(_image.DOFade(config.TargetAlpha, config.Duration)
                        .SetLoops(config.BlinkLoops, LoopType.Yoyo));
                }

                // Children Activation
                if (config.EnableChildrenOnComplete)
                {
                    sequence.OnComplete(ToggleChildren);
                }

                if (!_animationsDict.ContainsKey(config.Animation))
                {
                    _animationsDict[config.Animation] = new List<Tween>();
                }
                _animationsDict[config.Animation].Add(sequence);
            }
        }

        private void ToggleChildren()
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(!child.gameObject.activeSelf);
            }
        }

        public UIAnimationType GetAnimationType(AnimationTransitionID transitionID)
        {
            foreach (var config in _hologramConfigs)
            {
                if (config.AnimationTransitionID == transitionID)
                    return config.UIAnimationType;
            }

            Debug.LogError($"TransitionID {transitionID} not found in {gameObject.name}!");
            return UIAnimationType.SEQUENTIAL;
        }

        public AnimationTransitionID GetTransitionID(UIAnimationType animationType)
        {
            foreach (var config in _hologramConfigs)
            {
                if (config.UIAnimationType == animationType)
                {
                    return config.AnimationTransitionID;
                }
            }
            Debug.LogError($"TransitionID {animationType} not found in {gameObject.name}!");
            return AnimationTransitionID.NONE;
        }
    }

}
