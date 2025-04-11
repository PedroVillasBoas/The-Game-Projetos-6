using UnityEngine;
using GoodVillageGames.Game.Core.Manager.UI;
using GoodVillageGames.Game.Core.Util.Timer;
using GoodVillageGames.Game.Core.Attributes;
using GoodVillageGames.Game.Core.Manager.Player;
using GoodVillageGames.Game.Core.GameObjectEntity;


namespace GoodVillageGames.Game.Handlers
{
    public class BoostHandler : MonoBehaviour
    {
        private float _boostTotalTime;
        private float _boostTimeLeft;
        private float _boostRechargeRate;
        private CountdownTimer _countdownTimer;
        private StopwatchTimer _stopwatchTimer;
        private Stats _playerStats;
        private PlayerEventsManager _playerEventsManager;

        void OnEnable()
        {
            _playerEventsManager = transform.root.GetComponentInChildren<PlayerEventsManager>();
            _playerEventsManager.OnPlayerBoostingEventTriggered.AddListener(IsBoosting);
        }

        void OnDestroy()
        {
            _playerEventsManager.OnPlayerBoostingEventTriggered.RemoveListener(IsBoosting);
        }

        void Start()
        {
            _playerStats = transform.root.GetComponent<Entity>().Stats;
            _boostTotalTime = _playerStats.MaxBoostTime;
            _boostTimeLeft = _boostTotalTime;
            _boostRechargeRate = _playerStats.BoostRechargeRate;

            _countdownTimer = new(_boostTotalTime);
            _stopwatchTimer = new();

            _countdownTimer.OnTimerStop += () => _stopwatchTimer.Start();
        }

        void Update()
        {
            // Tick... Tack...
            _countdownTimer.Tick(Time.deltaTime);
            _stopwatchTimer.Tick(Time.deltaTime);

            // Use Boost
            if (_countdownTimer.IsRunning)
            {
                _boostTimeLeft = _countdownTimer.Time;
            }

            // Recharge Boost
            if (_stopwatchTimer.IsRunning)
            {
                float rechargeAmount = _boostRechargeRate * Time.deltaTime;
                _boostTimeLeft = Mathf.Min(_boostTimeLeft + rechargeAmount, _boostTotalTime);

                // Boost Full
                if (_boostTimeLeft >= _boostTotalTime)
                {
                    _stopwatchTimer.Stop();
                }
            }

            UIEventsManager.Instance.UpdateBoostUI(_boostTimeLeft / _boostTotalTime);
        }

        public void IsBoosting(bool value)
        {
            if (value)
            {
                if (_boostTimeLeft > 0)
                {
                    _countdownTimer.Reset(_boostTimeLeft);
                    _countdownTimer.Start();
                }
            }
            else
            {
                _countdownTimer.Stop();

                if (_boostTimeLeft < _boostTotalTime)
                    _stopwatchTimer.Start();
            }
        }
    }
}
