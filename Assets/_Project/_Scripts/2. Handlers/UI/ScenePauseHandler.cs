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

            switch (GlobalGameManager.Instance.GameState)
            {
                case GameState.GamePaused:
                    TogglePause();
                    break;

                case GameState.GameBegin:
                    TogglePause();
                    break;

                case GameState.GameContinue:
                    TogglePause();
                    break;

                default:
                    Debug.LogError($"Cannot pause the game when in state {GlobalGameManager.Instance.GameState}.");
                    break;
            }
        }

        private void TogglePause()
        {
            bool shouldPause = GlobalGameManager.Instance.GameState != GameState.GamePaused;
            Time.timeScale = shouldPause ? 0f : 1f;

            if (shouldPause)
                GlobalEventsManager.Instance.ChangeGameState(GameState.GamePaused);
            else
                GlobalEventsManager.Instance.ChangeGameState(GameState.GameContinue);
        }
    }
}
