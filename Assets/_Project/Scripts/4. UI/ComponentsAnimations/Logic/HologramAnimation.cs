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
            foreach (var _hologramAnimations in _hologramConfigs)
            {
                var sequence = DOTween.Sequence();

                // Size Animation
                sequence.Append(_rectTransform.DOSizeDelta(_hologramAnimations.FinalSize, _hologramAnimations.SizeChangeDuration)
                    .SetEase(Ease.InExpo));

                // Blink Effect
                if (_hologramAnimations.Blink)
                {
                    sequence.Join(_image.DOFade(_hologramAnimations.TargetAlpha, _hologramAnimations.Duration)
                        .SetLoops(_hologramAnimations.BlinkLoops, LoopType.Yoyo));
                }

                // Children Activation
                if (_hologramAnimations.EnableChildrenOnComplete)
                {
                    sequence.OnComplete(ToggleChildren);
                }

                if (!_animationsDict.ContainsKey(_hologramAnimations.Animation))
                {
                    _animationsDict[_hologramAnimations.Animation] = new List<Tween>();
                }
                _animationsDict[_hologramAnimations.Animation].Add(sequence);
            }
        }

        private void ToggleChildren()
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(!child.gameObject.activeSelf);
            }
        }

        public UIAnimationType GetAnimationType(UIAnimationType animationType)
        {
            foreach (var config in _hologramConfigs)
            {
                if (config.UIAnimationType == animationType)
                    return config.UIAnimationType;
            }

            Debug.LogError($"AnimationID {animationType} não encontrado neste componente!");
            return UIAnimationType.SEQUENTIAL;
        }

        public UIAnimationType GetAnimationType(AnimationTransitionID transitionID)
        {
            foreach (var config in _hologramConfigs)
            {
                if (config.AnimationTransitionID == transitionID)
                    return config.UIAnimationType;
            }

            Debug.LogError($"AnimationID {transitionID} não encontrado neste componente!");
            return UIAnimationType.SEQUENTIAL;
        }

        public AnimationTransitionID GetTransitionID(AnimationTransitionID transitionID)
        {
            foreach (var config in _hologramConfigs)
            {
                if (config.AnimationTransitionID == transitionID)
                    return config.AnimationTransitionID;
            }

            Debug.LogError($"AnimationTransitionID {transitionID} Not found in {gameObject.name}!");
            return AnimationTransitionID.NONE;
        }

        UIAnimationType IComponentAnimation.GetTransitionID(AnimationTransitionID transitionID)
        {
            foreach (var config in _hologramConfigs)
            {
                if (config.AnimationTransitionID == transitionID)
                    return config.UIAnimationType;
            }

            Debug.LogError($"AnimationTransitionID {transitionID} Not found in {gameObject.name}!");
            return UIAnimationType.SEQUENTIAL;
        }
    }

}
