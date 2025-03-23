using System;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using static GoodVillageGames.Game.Enums.Enums;
using GoodVillageGames.Game.Core.ScriptableObjects;


namespace GoodVillageGames.Game.Core.Manager
{
    public class EventsManager : MonoBehaviour
    {
        // Singleton
        public static EventsManager Instance { get; private set; }

        // Generic event
        public event Action<string> OnEventTriggered;

        // Component To Add
        public event Action<GameObject> OnAddComponentToStackTriggered;

        // Button Animation
        public event Action<AnimationID, SceneScriptableObject> OnButtonAnimationEventTriggered;

        // Animations 
        // State
        public event Action<UIState> OnAnimationEventTriggered;
            // Ask Animation
        public event Action<Sequence, AnimationID, SceneScriptableObject> OnAnimationAskedEventTriggered;
            // Broadcast Which Animation
        public event Action<AnimationID> OnPlayingAnimationEventTriggered;
            // Complete
        public event Action<SceneScriptableObject> OnChangeSceneEventTriggered;

        // Game State
        public event Action<GameState> OnGameStateEventTriggered;

        // Scene
            // Loaded
        public event Action<Scene, LoadSceneMode> OnSceneHasLoadedEventTriggered;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void TriggerEvent(string _event)
        {
            OnEventTriggered?.Invoke(_event);
        }

        public void AddComponentToStackTriggered(GameObject _gameObject)
        {
            OnAddComponentToStackTriggered?.Invoke(_gameObject);
        }

        public void ButtonAnimationEventTriggered(AnimationID _animationID, SceneScriptableObject _sceneSO = null)
        {
            OnButtonAnimationEventTriggered?.Invoke(_animationID, _sceneSO);
        }

        public void AnimationAskedEventTriggered(Sequence sequence, AnimationID _animationID = AnimationID.NONE, SceneScriptableObject _sceneSO = null)
        {
            OnAnimationAskedEventTriggered?.Invoke(sequence, _animationID,  _sceneSO);
        }

        public void PlayingAnimationEventTriggered(AnimationID _animationID)
        {
            OnPlayingAnimationEventTriggered?.Invoke(_animationID);
        }

        public void AnimationTriggerEvent(UIState _uiState)
        {
            OnAnimationEventTriggered?.Invoke(_uiState);
        }

        public void ChangeSceneTriggerEvent(SceneScriptableObject _sceneSO)
        {
            OnChangeSceneEventTriggered?.Invoke(_sceneSO);
        }

        public void GameStateTriggerEvent(GameState _gameState)
        {
            OnGameStateEventTriggered?.Invoke(_gameState);
        }

        public void SceneHasLoadedEventTriggered(Scene _scene, LoadSceneMode _mode)
        {
            OnSceneHasLoadedEventTriggered?.Invoke(_scene, _mode);
        }
    }
}
