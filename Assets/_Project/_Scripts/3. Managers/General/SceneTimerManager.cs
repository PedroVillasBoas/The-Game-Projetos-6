using UnityEngine;
using GoodVillageGames.Game.Core.Global;
using GoodVillageGames.Game.Core.Util.Timer;
using static GoodVillageGames.Game.Enums.Enums;

namespace GoodVillageGames.Game.Core.Manager
{
    /// <summary>
    /// This is responsable for the time the player has passed in a single run.
    /// </summary>
    public class SceneTimerManager : MonoBehaviour
    {
        public static SceneTimerManager Instance { get; private set; }

        private StopwatchTimer _runTimer = new();

        void Awake()
        {
            // Singleton
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        void OnEnable()
        {
            GlobalEventsManager.Instance.ChangeGameStateEventTriggered += OnGameStateChanged;
        }

        void Update()
        {
            _runTimer.Tick(Time.deltaTime);
        }

        void OnGameStateChanged(GameState gameState)
        {
            switch (gameState)
            {
                case GameState.GameBegin:
                    _runTimer.Start();
                    break;

                case GameState.GamePaused:
                case GameState.UpgradeScreen:
                case GameState.PlayerDied:
                    StopRunTimer();
                    break;

                case GameState.GameContinue:
                    ResumeRunTimer();
                    break;
            }
        }

        void ResumeRunTimer()
        {
            _runTimer.Resume();
        }

        void StopRunTimer()
        {
            _runTimer.Stop();
        }

        public float GetRunTime()
        {
            return _runTimer.GetTime();
        }
    }
}
