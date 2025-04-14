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
                if (config.ComponentToAnimate == null || (config.ComponentToAnimate2 == null && config.Blink))
                {
                    Debug.LogError($"Image and/or RectTranform not defined in {gameObject.name}");
                    return;
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
                    sequence.OnComplete(() => ToggleChildren(true));
                }

                if (config.DisableChildren)
                {
                    sequence.Insert(
                        config.DisableAt,
                        DOTween.To(() => 0, x => { }, 0, 0) // Empty Tween, just to deactivate on the right time
                            .OnComplete(() => ToggleChildren(false))
                    );
                }


                if (!_animationsDict.ContainsKey(config.AnimationTransitionID))
                {
                    _animationsDict[config.AnimationTransitionID] = new List<Tween>();
                }
                _animationsDict[config.AnimationTransitionID].Add(sequence);
            }
        }

        private void ToggleChildren(bool value)
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(value);
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
            return UIAnimationType.Sequential;
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

        public int GetInsertAtPosition(AnimationTransitionID transitionID)
        {
            foreach (var config in _hologramConfigs)
            {
                if (config.AnimationTransitionID == transitionID)
                {
                    return config.InsertPosition;
                }
            }
            Debug.LogError($"InsertPosition of {transitionID} not found in {gameObject.name}!");
            return 0; // Default value 
        }
    }

}
