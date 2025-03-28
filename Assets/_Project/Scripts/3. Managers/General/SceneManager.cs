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
        [SerializeField] List<AnimationTransitionID> _transicionsOnSceneList = new();
        [SerializeField] List<SceneScriptableObject> _scenesSOList = new();
        [SerializeField] private bool _playAnimationOnAwake = false;
        [SerializeField, ShowIf(nameof(_playAnimationOnAwake))] private AnimationTransitionID _animationIDToPlayOnSceneShow;

        private Dictionary<AnimationTransitionID, Tween> _sequenceDict = new();
        private Dictionary<AnimationTransitionID, SceneScriptableObject> _transitionsOnSceneDict;

        void OnEnable()
        {
            EventsManager.Instance.OnButtonAskingAnimationEventTriggered += PlayThisAnimation;
        }

        void OnDisable()
        {
            EventsManager.Instance.OnButtonAskingAnimationEventTriggered -= PlayThisAnimation;
        }

        void Start()
        {
            FillAnimationTransitionDict();
            PlayAnimationOnSceneShow();
        }

        void PlayAnimationOnSceneShow()
        {
            if (!_playAnimationOnAwake) return;

            if (!_transitionsOnSceneDict.ContainsKey(_animationIDToPlayOnSceneShow))
            {
                Debug.LogError($"ID {_animationIDToPlayOnSceneShow} does not exist in _transitionsOnSceneDict!");
                return;
            }

            if (_sequenceDict.ContainsKey(_animationIDToPlayOnSceneShow))
                PlayThisAnimation(_animationIDToPlayOnSceneShow);
            else
                Debug.LogError($"No animation found for ID: {_animationIDToPlayOnSceneShow}");
        }

        private void FillAnimationTransitionDict()
        {
            _transitionsOnSceneDict = new();

            if (_transicionsOnSceneList.Count != _scenesSOList.Count)
            {
                Debug.LogError($"{_transicionsOnSceneList} and {_scenesSOList} has different sizes on {gameObject.name}");
                return;
            }

            for (int i = 0; i < _transicionsOnSceneList.Count; i++)
            {
                var key = _transicionsOnSceneList[i];
                var value = _scenesSOList[i];


                if (key == AnimationTransitionID.NONE)
                {
                    Debug.LogError($"No Transition ID in {_transicionsOnSceneList} in position {i}!");
                    continue;
                }

                if (_transitionsOnSceneDict.ContainsKey(key))
                {
                    Debug.LogError($"Duplicate key: {key}");
                    continue;
                }

                _transitionsOnSceneDict.Add(key, value);
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
                int insertPosition = componentAnimation.GetInsertAtPosition(transitionID);

                Debug.Log($"Processando animação para ID: {transitionID}");
                ProcessAnimationTransition(transitionID, tweens, animType, insertPosition);
            }
        }

        private void ProcessAnimationTransition(AnimationTransitionID transitionID, List<Tween> tweens, UIAnimationType animType, int insertPosition = -1)
        {
            if (!_sequenceDict.TryGetValue(transitionID, out Tween sequence))
            {
                sequence = CreateNewSequence(transitionID);
            }

            AddTweensToSequence((Sequence)sequence, tweens, animType, insertPosition);
        }

        private Sequence CreateNewSequence(AnimationTransitionID transitionID)
        {
            Sequence newSequence = DOTween.Sequence();
            newSequence.SetAutoKill(false);
            _sequenceDict[transitionID] = newSequence;
            return newSequence;
        }

        private void AddTweensToSequence(Sequence sequence, List<Tween> tweens, UIAnimationType animType, int insertPosition)
        {
            foreach (Tween tween in tweens)
            {
                if (animType == UIAnimationType.SEQUENTIAL)
                {
                    sequence.Append(tween);
                }
                else
                {
                    if (insertPosition == 0)
                        sequence.Join(tween); // Parallel, don't care about the position
                    else
                        sequence.Insert(insertPosition, tween); // Specific Position
                }
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
                /* 
                    Opa peu do Futuro, peu do passado aqui, saca:
                        Ta dando erro porque ele não ta pegando as animações dos children desativados. Porque? Nao sei;
                        Ideias que eu tive:
                            1. Ver como funciona o async loading ou vai ter uma real tela de loading, onde todos os objects da proxima scene 
                                vão estar ativos, fazer as animações, quando tiver tudo pronto, ai muda a tela.
                            2. Criar um script pra colocar no parent dos objects que tem animação, o parent construir a sequence dos children,
                                juntar tudo e finalmente mandar pra cá, o SceneManager;
                */ 
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
    }
}
