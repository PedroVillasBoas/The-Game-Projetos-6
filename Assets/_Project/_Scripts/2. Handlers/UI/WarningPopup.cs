using TMPro;
using UnityEngine;
using GoodVillageGames.Game.Core.Global;
using GoodVillageGames.Game.Core.Util.Timer;

namespace GoodVillageGames.Game.Handlers.UI
{
    public class WarningPopup : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI timerText;

        private readonly float totalTime = 10f;
        private CountdownTimer timer;
        private bool gameOver = false;

        void OnEnable()
        {
            timer = new(totalTime);
            timer.Start();
        }
        void OnDisable() => timer.Stop();

        void Update()
        {
            if (gameOver) return;

            if (timer.Progress >= 1)
                MissionFailed();

            timer.Tick(Time.deltaTime);
            UpdateTextTimer();
        }

        void UpdateTextTimer()
        {
            float remainingTime = Mathf.Max(0, timer.Time);
            int seconds = Mathf.FloorToInt(remainingTime);
            int milliseconds = Mathf.FloorToInt((remainingTime - seconds) * 1000);

            timerText.text = $"{seconds:00}:{milliseconds:000}";
        }

        void MissionFailed()
        {
            if (gameOver)
            {
                GlobalEventsManager.Instance.ChangeGameState(Enums.Enums.GameState.PlayerDied);
                timer.Stop();
                timerText.text = "00:000";
            }
        }
    }
}
