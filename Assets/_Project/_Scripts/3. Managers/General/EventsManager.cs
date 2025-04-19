using System;
using UnityEngine;
using GoodVillageGames.Game.Core.ScriptableObjects;


namespace GoodVillageGames.Game.Core.Manager
{
    public class EventsManager : MonoBehaviour
    {
        // Singleton Instance
        public static EventsManager Instance { get; private set; }

        // Generic event
        public event Action<string> OnEventTriggered;
        
        // Scene Change
        public event Action<SceneScriptableObject> OnChangeSceneEventTriggered;

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

        // Change the Scene
        public void ChangeSceneTriggerEvent(SceneScriptableObject _sceneSO)
        {
            OnChangeSceneEventTriggered?.Invoke(_sceneSO);
        }

        public void SceneLoadedTriggerEvent()
        {
            OnSceneLoadedEventTriggered?.Invoke();
        }
    }
}
