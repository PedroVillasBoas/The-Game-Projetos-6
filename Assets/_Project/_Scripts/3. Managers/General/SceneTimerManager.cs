using GoodVillageGames.Game.Core.Global;
using GoodVillageGames.Game.Core.Util.Timer;
using UnityEngine;

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
            GlobalEventsManager.Instance.StartRunEventTriggered += StartRunTimer;
            GlobalEventsManager.Instance.EndRunEventTriggered += StopRunTimer;
        }

        void Update()
        {
            _runTimer.Tick(Time.deltaTime);
        }

        void StartRunTimer()
        {
            _runTimer.Start();
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
