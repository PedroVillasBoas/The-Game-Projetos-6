using System;
using UnityEngine;
using UnityEngine.InputSystem;
using GoodVillageGames.Game.Core.Global;
using static GoodVillageGames.Game.Enums.Enums;

namespace GoodVillageGames.Game.Handlers.UI
{
    /// <summary>
    /// Handles toggling the game pause state and notifies listeners.
    /// Objects that should ignore pause can subscribe to OnPauseToggle and use unscaled time.
    /// </summary>
    public class ScenePauseHandler : MonoBehaviour 
    {
        [SerializeField] private InputActionReference pauseAction;

        public static ScenePauseHandler Instance { get; private set; }

        void Awake()
        {
            // Scene Singleton
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        private void OnEnable()
        {
            pauseAction.action.performed += OnPauseAsked;
        }

        private void OnDisable()
        {
            pauseAction.action.performed -= OnPauseAsked;
        }

        private void OnPauseAsked(InputAction.CallbackContext context)
        {
            if (!context.performed)
                return;
            if (GlobalGameManager.Instance.UIState != UIState.PLAYING_UI_ANIM)
            {
                switch (GlobalGameManager.Instance.GameState)
                {
                    case GameState.GamePaused:
                    case GameState.GameBegin:
                    case GameState.GameContinue:
                        TogglePause();
                        break;

                    default:
                        Debug.LogError($"Cannot pause the game when in state {GlobalGameManager.Instance.GameState}.");
                        break;
                }

            }
        }

        void TogglePause()
        {
            bool shouldPause = GlobalGameManager.Instance.GameState != GameState.GamePaused;

            if (shouldPause)
            {
                GlobalEventsManager.Instance.ChangeGameState(GameState.GamePaused);
                Time.timeScale = 0f;
            }
            else
            {
                GlobalEventsManager.Instance.ChangeGameState(GameState.GameContinue);
                Time.timeScale = 1f;
            }
        }

        public void PauseTimeScale()
        {
            Time.timeScale = 0f;
        }

        public void ReturnToOriginalTimeScale()
        {
            Time.timeScale = 1f;
        }
    }
}
