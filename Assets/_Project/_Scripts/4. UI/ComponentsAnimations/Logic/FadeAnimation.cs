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
                if (config.ComponentToAnimate == null)
                {
                    Debug.LogError($"Component not defined in {gameObject.name}");
                    continue;
                }

                // Tween
                Tween tween = _image.DOFade(config.TargetAlpha, config.Duration)
                    .SetAutoKill(false);

                // Add to Dict
                if (!_animationsDict.ContainsKey(config.AnimationTransitionID))
                {
                    _animationsDict[config.AnimationTransitionID] = new List<Tween>();
                }
                _animationsDict[config.AnimationTransitionID].Add(tween);
            }
        }

        public UIAnimationType GetAnimationType(AnimationTransitionID transitionID)
        {
            foreach (var config in _fadeConfigs)
            {
                if (config.AnimationTransitionID == transitionID)
                    return config.UIAnimationType;
            }

            Debug.LogError($"TransitionID {transitionID} not found in {gameObject.name}!");
            return UIAnimationType.Sequential;
        }

        public AnimationTransitionID GetTransitionID(UIAnimationType animationType)
        {
            foreach (var config in _fadeConfigs)
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
            foreach (var config in _fadeConfigs)
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