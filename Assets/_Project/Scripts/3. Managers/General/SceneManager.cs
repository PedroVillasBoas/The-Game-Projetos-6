using UnityEngine;
using DG.Tweening;
using TriInspector;
using System.Collections.Generic;
using GoodVillageGames.Game.Interfaces;
using static GoodVillageGames.Game.Enums.Enums;
using GoodVillageGames.Game.Core.ScriptableObjects;

namespace GoodVillageGames.Game.Core.Manager
{
    public class SceneManager : MonoBehaviour
    {
        [SerializeField] List<AnimationTransitionID> _transicionsOnScene = new();
        [SerializeField] List<SceneScriptableObject> _scenesSO = new();
        [SerializeField] private bool _playAnimationOnAwake = false;
        [SerializeField, ShowIf(nameof(_playAnimationOnAwake))] private AnimationTransitionID _animationIDToPlayOnSceneShow;

        private Dictionary<AnimationTransitionID, Tween> _sequenceDict = new();
        private Dictionary<AnimationTransitionID, SceneScriptableObject> _transitionsOnSceneDict;

        void Start()
        {
            FillAnimationTransitionDict();
            PlayAnimationOnSceneShow();
        }

        void PlayAnimationOnSceneShow()
        {
            if (_playAnimationOnAwake && _sequenceDict.ContainsKey(_animationIDToPlayOnSceneShow))
            {
                PlayThisAnimation(_animationIDToPlayOnSceneShow);
            }
        }

        private void FillAnimationTransitionDict()
        {
            _transitionsOnSceneDict = new();

            for (int i = 0; i < _transicionsOnScene.Count; i++)
            {
                var key = _transicionsOnScene[i];
                var value = _scenesSO[i];

                if (key != AnimationTransitionID.NONE)
                {
                    _transitionsOnSceneDict.Add(key, value);
                }
            }

            GetAllAnimationsInScene();
        }

        private void GetAllAnimationsInScene()
        {
            MonoBehaviour[] allMonoBehaviors = FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None);

            foreach (MonoBehaviour monoB in allMonoBehaviors)
            {
                if (monoB is IComponentAnimation componentAnimation)
                {
                    OccupyAnimationsDict(componentAnimation);
                }
            }
        }

        private void OccupyAnimationsDict(IComponentAnimation componentAnimation)
        {
            foreach (var animationPair in componentAnimation.Animations)
            {
                AnimationTransitionID transitionID = animationPair.Key;
                List<Tween> tweens = animationPair.Value;
                UIAnimationType animType = componentAnimation.GetAnimationType(transitionID);

                ProcessAnimationTransition(transitionID, tweens, animType);
            }
        }

        private void ProcessAnimationTransition(AnimationTransitionID transitionID, List<Tween> tweens, UIAnimationType animType)
        {
            if (!_sequenceDict.TryGetValue(transitionID, out Tween sequence))
            {
                sequence = CreateNewSequence(transitionID);
            }

            AddTweensToSequence((Sequence)sequence, tweens, animType);
        }

        private Sequence CreateNewSequence(AnimationTransitionID transitionID)
        {
            Sequence newSequence = DOTween.Sequence();
            newSequence.SetAutoKill(false);
            _sequenceDict[transitionID] = newSequence;
            return newSequence;
        }

        private void AddTweensToSequence(Sequence sequence, List<Tween> tweens, UIAnimationType animType)
        {
            foreach (Tween tween in tweens)
            {
                if (animType == UIAnimationType.SEQUENTIAL)
                    sequence.Append(tween);
                else
                    sequence.Join(tween);
            }
        }

        private void PlayThisAnimation(AnimationTransitionID _animationTransitionID)
        {
            if (_sequenceDict.TryGetValue(_animationTransitionID, out var value))
            {
                if (value == null)
                    Debug.LogError($"Key '{_animationTransitionID}' exists but has no value (null) in Sequence Dictionary.");
                else
                {
                    CheckSceneInDictionary(_animationTransitionID, value);
                }
            }
            else
            {
                Debug.LogError($"Key '{_animationTransitionID}' does not exist in Sequence Dictionary.");
            }
        }

        private void CheckSceneInDictionary(AnimationTransitionID _animationTransitionID, Tween value)
        {
            if (_transitionsOnSceneDict.TryGetValue(_animationTransitionID, out SceneScriptableObject scene))
                EventsManager.Instance.AnimationAskedEventTriggered((Sequence)value, _animationTransitionID, scene);
            else
                Debug.LogError($"Key '{_animationTransitionID}' exists but has no value (null) in Transitions Dictonary.");
        }

        void OnEnable()
        {
            EventsManager.Instance.OnButtonAskingAnimationEventTriggered += PlayThisAnimation;
        }

        void OnDisable()
        {
            EventsManager.Instance.OnButtonAskingAnimationEventTriggered -= PlayThisAnimation;
        }

    }
}
