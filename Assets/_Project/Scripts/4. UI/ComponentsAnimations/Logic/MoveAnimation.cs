using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using GoodVillageGames.Game.Interfaces;
using static GoodVillageGames.Game.Enums.Enums;
using GoodVillageGames.Game.General.UI.Animations.Config;

namespace GoodVillageGames.Game.General.UI.Animations
{
    [RequireComponent(typeof(RectTransform))]
    public class MoveAnimation : MonoBehaviour, IComponentAnimation
    {
        [SerializeField] private List<MoveAnimationConfig> _moveConfigs = new();

        private RectTransform _rectTranform;
        private Dictionary<AnimationTransitionID, List<Tween>> _animationsDict = new();

        public Dictionary<AnimationTransitionID, List<Tween>> Animations => _animationsDict;

        void Awake()
        {
            _rectTranform = GetComponent<RectTransform>();
            BuildAnimations();
        }

        public void BuildAnimations()
        {
            foreach (var config in _moveConfigs)
            {
                // Validating...
                Component target = config.ComponentToAnimate switch
                {
                    RectTransform rect => _rectTranform,
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
                    RectTransform rec => rec.DOAnchorPos(config.FinalPosition, config.Duration),
                    _ => null
                };

                if (tween != null)
                {
                    _animationsDict[config.Animation].Add(tween);
                }
            }
        }


        public UIAnimationType GetAnimationType(UIAnimationType animationType)
        {
            foreach (var config in _moveConfigs)
            {
                if (config.UIAnimationType == animationType)
                    return config.UIAnimationType;
            }

            Debug.LogError($"AnimationID {animationType} não encontrado neste componente!");
            return UIAnimationType.SEQUENTIAL;
        }

        public UIAnimationType GetAnimationType(AnimationTransitionID transitionID)
        {
            foreach (var config in _moveConfigs)
            {
                if (config.AnimationTransitionID == transitionID)
                    return config.UIAnimationType;
            }

            Debug.LogError($"AnimationID {transitionID} não encontrado neste componente!");
            return UIAnimationType.SEQUENTIAL;
        }

        public AnimationTransitionID GetTransitionID(AnimationTransitionID transitionID)
        {
            foreach (var config in _moveConfigs)
            {
                if (config.AnimationTransitionID == transitionID)
                    return config.AnimationTransitionID;
            }

            Debug.LogError($"AnimationTransitionID {transitionID} Not found in {gameObject.name}!");
            return AnimationTransitionID.NONE;
        }

        UIAnimationType IComponentAnimation.GetTransitionID(AnimationTransitionID transitionID)
        {
            foreach (var config in _moveConfigs)
            {
                if (config.AnimationTransitionID == transitionID)
                    return config.UIAnimationType;
            }

            Debug.LogError($"AnimationTransitionID {transitionID} Not found in {gameObject.name}!");
            return UIAnimationType.SEQUENTIAL;
        }
    }
}
