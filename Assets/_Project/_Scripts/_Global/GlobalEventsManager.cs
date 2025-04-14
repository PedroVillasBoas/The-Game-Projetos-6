using System;
using UnityEngine;

namespace GoodVillageGames.Game.Core.Global
{
    /// <summary>
    /// This is the Global Class that handle the communication between major systems of the game;
    /// </summary>
    public class GlobalEventsManager : MonoBehaviour 
    {
        // Singleton
        public static GlobalEventsManager Instance { get; private set; }

        // Events
        public event Action ChangeGameStateEventTriggered;

        public event Action StartTutorialEventTriggered;
        public event Action StartRunEventTriggered;
        public event Action EndRunEventTriggered;

        void Awake()
        {
            // Singleton
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);

            DontDestroyOnLoad(gameObject);
        }

        // Temp
        public void StartGame()
        {
            StartRunEventTriggered?.Invoke();
        }
    }
}
