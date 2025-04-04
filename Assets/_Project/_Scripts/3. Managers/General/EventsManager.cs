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
        
        // Scene Change
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
