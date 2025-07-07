using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;
using UnityEngine.InputSystem;
using GoodVillageGames.Game.Enums;
using GoodVillageGames.Game.Enums.UI;
using GoodVillageGames.Game.Core.Global;
using UnityEngine.Rendering.Universal;

namespace GoodVillageGames.Game.Handlers.UI
{
    /// <summary>
    /// Handles toggling the game pause state and notifies listeners.
    /// Objects that should ignore pause can subscribe to OnPauseToggle and use unscaled time.
    /// </summary>
    public class ScenePauseHandler : MonoBehaviour
    {
        [SerializeField] private InputActionReference pauseAction;

        [SerializeField] private Volume worldVolume;
        [SerializeField] private Volume uiVolume;

        // UI PP properties
        private Bloom uiBloom;
        private Vignette uiVignette;
        private LensDistortion uiLensDistortion;
        private ChromaticAberration uiChromaticAberration;

        public static ScenePauseHandler Instance { get; private set; }

        private bool isProcessingPause;

        void Awake()
        {
            // Scene Singleton
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);

            if (uiVolume == null)
            {
                Debug.LogError("ScenePauseHandler: Post Process Volume is not assigned!");
                return;
            }

            uiVolume.profile.TryGet(out uiBloom);
            uiVolume.profile.TryGet(out uiLensDistortion);
            uiVolume.profile.TryGet(out uiVignette);
            uiVolume.profile.TryGet(out uiChromaticAberration);

            PauseTimeScale();
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

            try
            {
                switch (GlobalGameManager.Instance.GameState)
                {
                    case GameState.GameBegin:
                    case GameState.GamePaused:
                    case GameState.GameContinue:
                        TogglePause();
                        break;


                    default:
                        Debug.LogError($"Cannot pause in {GlobalGameManager.Instance.GameState}");
                        break;
                }
            }
            finally
            {
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
                PauseWorldPostProcessing(true);
                PauseUIPostProcessing(true);
                Time.timeScale = 0f;
            }
            else
            {
                GlobalEventsManager.Instance.ChangeGameState(GameState.GameContinue);
                PauseWorldPostProcessing(false);
                PauseUIPostProcessing(false);
                Time.timeScale = 1f;
            }
        }

        void PauseWorldPostProcessing(bool isPaused)
        {
            worldVolume.enabled = !isPaused;
        }

        void PauseUIPostProcessing(bool isPaused)
        {
            if (isPaused)
            {
                uiBloom.active = true;
                uiLensDistortion.active = true;
                uiVignette.active = true;

                uiChromaticAberration.intensity.value = 0.25f;
            }
            else
            {
                uiBloom.active = false;
                uiLensDistortion.active = false;
                uiVignette.active = false;

                uiChromaticAberration.intensity.value = 0.09f;
            }
        }

        public void PauseTimeScale()
        {
            PauseWorldPostProcessing(true);
            PauseUIPostProcessing(true);
            Time.timeScale = 0f;
        }

        public void ReturnToOriginalTimeScale()
        {
            PauseWorldPostProcessing(false);
            PauseUIPostProcessing(false);
            Time.timeScale = 1f;
        }
    }
}
