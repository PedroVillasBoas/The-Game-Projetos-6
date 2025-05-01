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
        private bool firstLogin = true;

        public GameState GameState { get => gameState; set => gameState = value; }
        public UIState UIState { get => uIState; set => uIState = value; }
        public GameDifficulty CurrentDifficulty { get => currentDifficulty; set => currentDifficulty = value; }
        public bool FirstLogin { get => firstLogin; set => firstLogin = value; }

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
            GlobalEventsManager.Instance.OnAnimationEventTriggered += OnUIAnimationStateChange;
        }

        void OnChangeState(GameState state)
        {
            gameState = state;
        }

        void OnUIAnimationStateChange(UIState state)
        {
            uIState = state;
        }
    }
}
