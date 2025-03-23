using UnityEngine;
using DG.Tweening;
using TriInspector;
using System.Collections.Generic;
using GoodVillageGames.Game.Interfaces;
using static GoodVillageGames.Game.Enums.Enums;
using GoodVillageGames.Game.Core.ScriptableObjects;
using GoodVillageGames.Game.General.UI;

namespace GoodVillageGames.Game.Core.Manager
{
    public class SceneManager : MonoBehaviour
    {

        [SerializeField] private bool _playAnimationOnAwake = false;
        [SerializeField, ShowIf(nameof(_playAnimationOnAwake))]  private AnimationID _animationIDToPlayOnSceneShow;

        private Stack<IComponentAnimation> _componentAnimationStack = new();
        private Dictionary<AnimationID, SequenceActionType> _sequenceDict = new();


        void Start()
        {
            FillAnimationsDict();
            PlayAnimationOnSceneShow();
        }

        void PlayAnimationOnSceneShow()
        {
            if (_playAnimationOnAwake && _sequenceDict.ContainsKey(_animationIDToPlayOnSceneShow))
            {
                PlayThisAnimation(_animationIDToPlayOnSceneShow);
            }
        }

        private void FillAnimationsDict()
        {
            foreach (IComponentAnimation item in _componentAnimationStack)
            {
                AnimationID id = item.AnimationID;
                Tween tweener = item.ComponentTween;

                if (!_sequenceDict.TryGetValue(id, out SequenceActionType sequenceActionType))
                {
                    Sequence sequence = DOTween.Sequence();
                    sequence.Append(tweener); // Required for all new sequences

                    sequenceActionType = new SequenceActionType(sequence);
                    _sequenceDict.Add(id, sequenceActionType);
                }
                else
                {
                    // Append/Join based on animation type if there's a sequence already (:
                    if (item.UIAnimationType == UIAnimationType.SEQUENTIAL)
                    {
                        sequenceActionType.Sequence.Append(tweener);
                    }
                    else
                    {
                        sequenceActionType.Sequence.Join(tweener);
                    }
                }
            }

            _componentAnimationStack.Clear();
        }

        private void PlayThisAnimation(AnimationID animationID, SceneScriptableObject scene = null)
        {
            if (_sequenceDict.TryGetValue(animationID, out var value))
            {
                if (value == null)
                    Debug.LogError($"Key '{animationID}' exists but has no value (null).");
                else
                    EventsManager.Instance.AnimationAskedEventTriggered(value.Sequence, scene);
            }
            else
            {
                Debug.LogError($"Key '{animationID}' does not exist in the dictionary.");
            }
        }

        void AddAnimationsToStack(GameObject _gameObject)
        {
            if (_gameObject.TryGetComponent<IComponentAnimation>(out var componentAnimation))
            {
                _componentAnimationStack.Push(componentAnimation);
            }
            else if (_gameObject.TryGetComponent<IButtonAction>(out var buttonActionType))
            {
                AnimationID animationID = buttonActionType.AnimationID;

                if (_sequenceDict.TryGetValue(animationID, out SequenceActionType existingSequenceAction))
                {
                    existingSequenceAction.UIButtomActionType = buttonActionType.SequenceActionType.UIButtomActionType;
                }
            }

        }

        void OnEnable()
        {
            EventsManager.Instance.OnButtonAnimationEventTriggered += PlayThisAnimation;
            EventsManager.Instance.OnAddComponentToStackTriggered += AddAnimationsToStack;
        }

        void OnDisable()
        {
            EventsManager.Instance.OnButtonAnimationEventTriggered -= PlayThisAnimation;
            EventsManager.Instance.OnAddComponentToStackTriggered -= AddAnimationsToStack;
        }

    }
}
