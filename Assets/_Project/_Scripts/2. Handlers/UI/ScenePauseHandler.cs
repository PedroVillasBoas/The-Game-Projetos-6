using System;
using UnityEngine;
using System.Collections;
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

        private bool isProcessingPause;

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
            if (!context.performed || isProcessingPause) return;
            if (GlobalGameManager.Instance.UIState == UIState.PLAYING_UI_ANIM) return;

            isProcessingPause = true;

            try{
                switch (GlobalGameManager.Instance.GameState)
                {
                    case GameState.GamePaused:
                    case GameState.GameContinue:
                        TogglePause();
                        break;

                    case GameState.GameBegin: // temp
                        TogglePause();
                        break;

                    default:
                        Debug.LogError($"Cannot pause in {GlobalGameManager.Instance.GameState}");
                        break;
                }
            }
            finally{
                StartCoroutine(ResetPauseProcessing());
            }
        }

        IEnumerator ResetPauseProcessing()
        {
            yield return new WaitForSecondsRealtime(0.1f);
            isProcessingPause = false;
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
