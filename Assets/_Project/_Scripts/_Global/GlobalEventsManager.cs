using System;
using UnityEngine;
using GoodVillageGames.Game.Enums;
using GoodVillageGames.Game.Enums.UI;
using GoodVillageGames.Game.Enums.Enemy;
using GoodVillageGames.Game.Core.Attributes.Modifiers;


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
        public event Action<bool> TutorialChoiceEventTriggered;

        // UI Animations State
        public event Action<UIState> OnAnimationEventTriggered;

        // Game Difficulty
        public event Action<GameDifficulty> OnGameDifficultyEventTriggered;

        // Data Collection
        public event Action<EnemyType> EnemyDefeatedEventTriggered;
        public event Action<UpgradeStatModifier> UpgradeCollectedEventTriggered;

        // Language / Locale
        public event Action<int> ChangeLocaleEventTriggered;

        void Awake()
        {
            // Singleton
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);

            DontDestroyOnLoad(gameObject);
        }

        // Public Methods | Other Classes will invoke using this methods
        public void ChangeGameState(GameState gameState)                            =>      ChangeGameStateEventTriggered?.Invoke(gameState);
        public void ChangeTutorialChoice(bool choice)                               =>      TutorialChoiceEventTriggered?.Invoke(choice);
        public void AnimationTriggerEvent(UIState uiState)                          =>      OnAnimationEventTriggered?.Invoke(uiState);
        public void GameDifficultyTriggerEvent(GameDifficulty difficulty)           =>      OnGameDifficultyEventTriggered?.Invoke(difficulty);
        public void AddDefeatedEnemy(EnemyType enemyType)                           =>      EnemyDefeatedEventTriggered?.Invoke(enemyType);
        public void CollectUpgradeData(UpgradeStatModifier upgradeStatModifier)     =>      UpgradeCollectedEventTriggered?.Invoke(upgradeStatModifier);
        public void ChangeLocale(int _localeID)                                     =>      ChangeLocaleEventTriggered?.Invoke(_localeID);
    }
}
