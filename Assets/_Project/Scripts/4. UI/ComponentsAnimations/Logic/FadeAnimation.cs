using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using GoodVillageGames.Game.Interfaces;
using GoodVillageGames.Game.General.UI.Animations.Config;
using static GoodVillageGames.Game.Enums.Enums;
using UnityEngine.UI;

namespace GoodVillageGames.Game.General.UI.Animations
{
    [RequireComponent(typeof(Image))]
    public class FadeAnimation : MonoBehaviour, IComponentAnimation
    {
        [SerializeField] private List<FadeAnimationConfig> _fadeConfigs = new();

        private Image _image;
        private Dictionary<AnimationTransitionID, List<Tween>> _animationsDict = new();

        public Dictionary<AnimationTransitionID, List<Tween>> Animations => _animationsDict;

        void Awake()
        {
            _image = GetComponent<Image>();
            BuildAnimations();
        }

        public void BuildAnimations()
        {
            foreach (var config in _fadeConfigs)
            {
                // Validating...
                Component target = config.ComponentToAnimate switch
                {
                    Image img => _image,
                    _ => null
                };

                if (target == null)
                {
                    Debug.LogError($"Invalid Component to Fade: {gameObject.name}");
                    continue;
                }

                // Making Tween
                Tween tween = target switch
                {
                    Image img => img.DOFade(config.TargetAlpha, config.Duration),
                    _ => null
                };

                if (tween != null)
                {
                    _animationsDict[config.AnimationTransitionID].Add(tween);
                }
            }
        }

        public UIAnimationType GetAnimationType(UIAnimationType animationType)
        {
            foreach (var config in _fadeConfigs)
            {
                if (config.UIAnimationType == animationType)
                    return config.UIAnimationType;
            }

            Debug.LogError($"AnimationID {animationType} não encontrado neste componente!");
            return UIAnimationType.SEQUENTIAL;
        }

        public UIAnimationType GetAnimationType(AnimationTransitionID transitionID)
        {
            foreach (var config in _fadeConfigs)
            {
                if (config.AnimationTransitionID == transitionID)
                    return config.UIAnimationType;
            }

            Debug.LogError($"AnimationID {transitionID} não encontrado neste componente!");
            return UIAnimationType.SEQUENTIAL;
        }

        public AnimationTransitionID GetTransitionID(AnimationTransitionID transitionID)
        {
            foreach (var config in _fadeConfigs)
            {
                if (config.AnimationTransitionID == transitionID)
                    return config.AnimationTransitionID;
            }

            Debug.LogError($"AnimationTransitionID {transitionID} Not found in {gameObject.name}!");
            return AnimationTransitionID.NONE;
        }

        UIAnimationType IComponentAnimation.GetTransitionID(AnimationTransitionID transitionID)
        {
            foreach (var config in _fadeConfigs)
            {
                if (config.AnimationTransitionID == transitionID)
                    return config.UIAnimationType;
            }

            Debug.LogError($"AnimationTransitionID {transitionID} Not found in {gameObject.name}!");
            return UIAnimationType.SEQUENTIAL;
        }
    }
}