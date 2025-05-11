using FMODUnity;
using UnityEngine;
using GoodVillageGames.Game.Enums;
using GoodVillageGames.Game.Enums.Audio;
using GoodVillageGames.Game.Handlers.UI.Audio;

namespace GoodVillageGames.Game.Core.Global
{
    public class GlobalMusicManager : MonoBehaviour
    {
        public static GlobalMusicManager Instance { get; private set; }

        private GlobalAudioManager globalAudioManager;

        void Awake()
        {
            // Singleton
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);

            DontDestroyOnLoad(gameObject);

            globalAudioManager = GlobalAudioManager.Instance;
        }

        void OnEnable() => GlobalEventsManager.Instance.ChangeGameStateEventTriggered += OnGameStateChanged;
        void OnDestroy() => GlobalEventsManager.Instance.ChangeGameStateEventTriggered -= OnGameStateChanged;

        void OnGameStateChanged(GameState gameState)
        {
            switch (gameState)
            {
                case GameState.SplashScreen:
                    SetMusic(GameSceneAudio.SplashScreen);
                    break;

                case GameState.MainMenu:
                    SetMusic(GameSceneAudio.MainMenu);
                    InitializeAmbientAudio();
                    break;

                case GameState.Tutorial:
                    SetMusic(GameSceneAudio.Tutorial);
                    SetAmbient(GameSceneAudio.InGame);
                    break;

                case GameState.GameBegin:
                    SetMusic(GameSceneAudio.InGame);
                    //SetAudioLowPassFilter(GameAudioFilter.InGame);
                    break;

                // case GameState.GameContinue:
                //     SetAudioLowPassFilter(GameAudioFilter.InGame);
                //     break;

                // case GameState.GamePaused:
                //     SetAudioLowPassFilter(GameAudioFilter.Paused);
                //     break;

                case GameState.GameOver:
                    SetMusic(GameSceneAudio.GameOver);
                    SetAmbient(GameSceneAudio.GameOver);
                    break;

                default:
                    Debug.LogWarning($"Shouldn't change music in {gameState} State.\n Continuing...");
                    break;
            }
        }

        public void InitializeMusic()
        {
            // Music shouldn't be Cleaned by Global Audio Manager
            globalAudioManager.MusicEventInstance = RuntimeManager.CreateInstance(FMODEventsHandler.Instance.GameMusic);
            globalAudioManager.MusicEventInstance.start();
        }

        public void InitializeAmbientAudio()
        {
            globalAudioManager.AmbientEventInstance = RuntimeManager.CreateInstance(FMODEventsHandler.Instance.AmbientAudio);
            globalAudioManager.AmbientEventInstance.start();
        }

        void SetMusic(GameSceneAudio gameAudio)
        {
            globalAudioManager.MusicEventInstance.setParameterByName("MusicParameter", (float)gameAudio);
        }

        void SetAmbient(GameSceneAudio gameAudio)
        {
            globalAudioManager.AmbientEventInstance.setParameterByName("AmbientParameter", (float)gameAudio);
        }

        void SetAudioLowPassFilter(GameAudioFilter gameState)
        {
            globalAudioManager.MusicEventInstance.setParameterByName("MusicParameter", (float)gameState);
            globalAudioManager.AmbientEventInstance.setParameterByName("AmbientParameter", (float)gameState);
        }
    }
}
