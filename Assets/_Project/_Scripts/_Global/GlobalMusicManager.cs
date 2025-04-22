using FMODUnity;
using UnityEngine;
using FMOD.Studio;
using GoodVillageGames.Game.Handlers.UI.Audio;
using static GoodVillageGames.Game.Enums.Enums;

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
        void Start() => InitializeMusic();

        void OnGameStateChanged(GameState gameState)
        {
            switch (gameState)
            {
                case GameState.SplashScreen:
                    SetMusic(GameMusics.SplashScreen);
                    break;
                case GameState.MainMenu:
                    SetMusic(GameMusics.MainMenu);
                    break;
                case GameState.Tutorial:
                    SetMusic(GameMusics.InGame);
                    break;
                case GameState.GameOver:
                    SetMusic(GameMusics.GameOver);
                    break;
                default:
                    Debug.LogWarning($"Shouldn't change music in {gameState} State.\n Continuing...");
                    break;
            }
        }

        void InitializeMusic()
        {
            // Music shouldn't be Cleaned by Global Audio Manager
            globalAudioManager.MusicEventInstance = RuntimeManager.CreateInstance(FMODEventsHandler.Instance.GameMusic);
            globalAudioManager.MusicEventInstance.start();
        }

        public void SetMusic(GameMusics gameMusic)
        {
            globalAudioManager.MusicEventInstance.setParameterByName("MusicParameter", (float)gameMusic);
        }
    }
}
