using UnityEngine;
using GoodVillageGames.Game.Enums;
using GoodVillageGames.Game.Enums.UI;

namespace GoodVillageGames.Game.Core.Global
{
    /// <summary>
    /// This is a Global Class that controls the game and it's states;
    /// </summary>
    public class GlobalGameManager : MonoBehaviour
    {
        public static GlobalGameManager Instance { get; private set; }

        private GameState gameState = GameState.SplashScreen;
        private UIState uIState = UIState.NoAnimationPlaying;
        private GameDifficulty currentDifficulty;
        private bool showTutorial = true;

        public GameState GameState { get => gameState; set => gameState = value; }
        public UIState UIState { get => uIState; set => uIState = value; }
        public GameDifficulty CurrentDifficulty { get => currentDifficulty; private set => currentDifficulty = value; }
        public bool ShowTutorial { get => showTutorial; set => showTutorial = value; }

        void Awake()
        {
            // Singleton
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);

            DontDestroyOnLoad(gameObject);
        }

        void Start()
        {
            GlobalEventsManager.Instance.ChangeGameStateEventTriggered += OnChangeState;
            GlobalEventsManager.Instance.TutorialChoiceEventTriggered += OnTutorialChoice;
            GlobalEventsManager.Instance.OnAnimationEventTriggered += OnUIAnimationStateChange;
            GlobalEventsManager.Instance.OnGameDifficultyEventTriggered += OnChangeDifficulty;
        }

        void OnDestroy()
        {
            GlobalEventsManager.Instance.ChangeGameStateEventTriggered -= OnChangeState;
            GlobalEventsManager.Instance.TutorialChoiceEventTriggered -= OnTutorialChoice;
            GlobalEventsManager.Instance.OnAnimationEventTriggered -= OnUIAnimationStateChange;
            GlobalEventsManager.Instance.OnGameDifficultyEventTriggered -= OnChangeDifficulty;
        }

        void OnChangeState(GameState state) => gameState = state;
        void OnTutorialChoice(bool choice) => ShowTutorial = choice;
        void OnUIAnimationStateChange(UIState state) => uIState = state;
        void OnChangeDifficulty(GameDifficulty difficulty) => currentDifficulty = difficulty;
    }
}
