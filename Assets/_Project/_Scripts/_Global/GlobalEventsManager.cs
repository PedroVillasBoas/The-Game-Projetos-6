using System;
using UnityEngine;
using static GoodVillageGames.Game.Enums.Enums;

namespace GoodVillageGames.Game.Core.Global
{
    /// <summary>
    /// This is the Global Class that handle the communication between major systems of the game;
    /// </summary>
    public class GlobalEventsManager : MonoBehaviour 
    {
        // Singleton
        public static GlobalEventsManager Instance { get; private set; }

        // Game State
        public event Action<GameState> ChangeGameStateEventTriggered;

        // UI Animations State
        public event Action<UIState> OnAnimationEventTriggered;

        // Level Up
        public event Action PlayerLevelUpEventTriggered;

        // Data Collection
        public event Action<EnemyType> EnemyDefeatedEventTriggered;

        void Awake()
        {
            // Singleton
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);

            DontDestroyOnLoad(gameObject);
        }

        // Game States
        public void ChangeGameState(GameState gameState)
        {
            ChangeGameStateEventTriggered?.Invoke(gameState);
        }

        public void AnimationTriggerEvent(UIState uiState)
        {
            OnAnimationEventTriggered?.Invoke(uiState);
        }

        public void PlayerLevelUp()
        {
            PlayerLevelUpEventTriggered?.Invoke();
        }

        public void AddDefeatedEnemy(EnemyType enemyType)
        {
            EnemyDefeatedEventTriggered?.Invoke(enemyType);
        }

        
    }
}
