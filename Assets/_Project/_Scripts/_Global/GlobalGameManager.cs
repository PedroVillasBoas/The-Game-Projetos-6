using UnityEngine;
using static GoodVillageGames.Game.Enums.Enums;

namespace GoodVillageGames.Game.Core.Global
{
    /// <summary>
    /// This is a Global Class that controls the game and it's states;
    /// </summary>
    public class GlobalGameManager : MonoBehaviour
    {
        public static GlobalGameManager Instance { get; private set; }

        private GameState gameState = GameState.MainMenu;
        private UIState uIState = UIState.NoAnimationPlaying;

        public GameState GameState { get => gameState; set => gameState = value; }
        public UIState UIState { get => uIState; set => uIState = value; }

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
