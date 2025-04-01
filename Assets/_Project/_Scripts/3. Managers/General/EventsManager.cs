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
        // Singleton Instance
        public static EventsManager Instance { get; private set; }

        // Generic event
        public event Action<string> OnEventTriggered;

        // Animations 
        // State
        public event Action<UIState> OnAnimationEventTriggered;
            // Ask Animation
        public event Action<Sequence, AnimationTransitionID, SceneScriptableObject> OnAnimationAskedEventTriggered;
            // Send Animation
        public event Action<AnimationTransitionID> OnButtonAskingAnimationEventTriggered;
            // Broadcast Which Animation
        public event Action<AnimationTransitionID> OnPlayingAnimationEventTriggered;
            // Complete
        public event Action<SceneScriptableObject> OnChangeSceneEventTriggered;

        // Game State
        public event Action<GameState> OnGameStateEventTriggered;

        // Scene Loading
        public event Action OnSceneLoadedEventTriggered;

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

        // Generalized
        public void TriggerEvent(string _event)
        {
            OnEventTriggered?.Invoke(_event);
        }

        // Sent to UI Animation Manager by Scene Manager
        public void AnimationAskedEventTriggered(Sequence sequence, AnimationTransitionID _animationTransitionID, SceneScriptableObject _sceneSO = null)
        {
            OnAnimationAskedEventTriggered?.Invoke(sequence, _animationTransitionID,  _sceneSO);
        }

        // Button Pressed, Play Animation
        public void ButtonAskingAnimationEventTriggered(AnimationTransitionID _animationTransitionID)
        {
            OnButtonAskingAnimationEventTriggered?.Invoke(_animationTransitionID);
        }

        // Broadcasting the currently animation
        public void PlayingAnimationEventTriggered(AnimationTransitionID _animationTransitionID)
        {
            OnPlayingAnimationEventTriggered?.Invoke(_animationTransitionID);
        }

        // Broadcast that is playing an UI animation
        public void AnimationTriggerEvent(UIState _uiState)
        {
            OnAnimationEventTriggered?.Invoke(_uiState);
        }

        // Change the Scene
        public void ChangeSceneTriggerEvent(SceneScriptableObject _sceneSO)
        {
            OnChangeSceneEventTriggered?.Invoke(_sceneSO);
        }

        // Change Game State
        public void GameStateTriggerEvent(GameState _gameState)
        {
            OnGameStateEventTriggered?.Invoke(_gameState);
        }

        public void SceneLoadedTriggerEvent()
        {
            OnSceneLoadedEventTriggered?.Invoke();
        }
    }
}
