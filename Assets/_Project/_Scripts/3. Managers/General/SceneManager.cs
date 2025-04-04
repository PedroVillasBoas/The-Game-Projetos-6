using UnityEngine;
using DG.Tweening;
using TriInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using GoodVillageGames.Game.Interfaces;
using static GoodVillageGames.Game.Enums.Enums;
using GoodVillageGames.Game.Core.ScriptableObjects;

namespace GoodVillageGames.Game.Core.Manager
{
    public class SceneManager : MonoBehaviour
    {

        public bool IsInitialized { get; private set; }

        [Header("Initialization Settings")]
        [SerializeField] private float _minLoadTime = 1f;

        [SerializeField] private List<AnimationTransitionID> _transicionsOnSceneList = new();
        [SerializeField] private List<SceneScriptableObject> _scenesSOList = new();
        [SerializeField] private bool _playAnimationOnAwake = false;
        [SerializeField, ShowIf(nameof(_playAnimationOnAwake))] private AnimationTransitionID _animationIDToPlayOnSceneShow;
        [SerializeField] private List<GameObject> _objectsAnimationsToDeactivate;

        private Dictionary<AnimationTransitionID, Tween> _sequenceDict = new();
        private Dictionary<AnimationTransitionID, SceneScriptableObject> _transitionsOnSceneDict;

        void OnEnable()
        {
            UnityEngine.SceneManagement.SceneManager.sceneUnloaded += PlayAnimationOnSceneShow;
        }

        void OnDisable()
        {
            UnityEngine.SceneManagement.SceneManager.sceneUnloaded -= PlayAnimationOnSceneShow;
        }

        void Start()
        {
            StartCoroutine(SceneInitializationRoutine());
        }

        private IEnumerator SceneInitializationRoutine()
        {
            float startTime = Time.realtimeSinceStartup;

            yield return StartCoroutine(InitializeCoreSystems());
            yield return StartCoroutine(SetupUIComponents());

            float enlapsedTime = Time.realtimeSinceStartup - startTime;
            if (enlapsedTime < _minLoadTime)
                yield return new WaitForSeconds(_minLoadTime - enlapsedTime);
            
            IsInitialized = true;
        }

        #region Initialization Steps

        private IEnumerator InitializeCoreSystems()
        {
            FillAnimationTransitionDict();
            yield return null;
        }

        private IEnumerator SetupUIComponents()
        {
            DeactivateAnimationsObjects();
            yield return null;
        }

        #endregion


        void PlayAnimationOnSceneShow(Scene scene = default)
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

        #region Animations Setup

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

        #endregion

        #region Setup UI

        private void DeactivateAnimationsObjects()
        {
            if (_objectsAnimationsToDeactivate != null)
            {
                foreach (GameObject go in _objectsAnimationsToDeactivate)
                {
                    go.SetActive(false);
                }

                EventsManager.Instance.SceneLoadedTriggerEvent();
            }
        }

        #endregion

        #region Animation Playing

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
                Debug.LogError($"Should be using the {gameObject.name} anymore.");
            else
                Debug.LogError($"Key '{_animationTransitionID}' exists but has no value (null) in Transitions Dictonary.");
        }

        #endregion
    }
}
