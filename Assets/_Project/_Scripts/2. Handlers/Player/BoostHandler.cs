using UnityEngine;
using GoodVillageGames.Game.Core.Manager.UI;
using GoodVillageGames.Game.Core.Attributes;
using GoodVillageGames.Game.Core.Manager.Player;
using GoodVillageGames.Game.Core.GameObjectEntity;

namespace GoodVillageGames.Game.Handlers
{
    public class BoostHandler : MonoBehaviour
    {
        private float _boostTimeLeft;
        private Stats _playerStats;
        private PlayerEventsManager _playerEventsManager;
        private bool _isBoosting;
        private bool _canBoost = true;

        void OnEnable()
        {
            _playerEventsManager = transform.root.GetComponentInChildren<PlayerEventsManager>();
            _playerEventsManager.OnPlayerBoostingEventTriggered.AddListener(IsBoosting);
        }

        void OnDestroy()
        {
            if (_playerEventsManager != null)
                _playerEventsManager.OnPlayerBoostingEventTriggered.RemoveListener(IsBoosting);
        }

        void Start()
        {
            _playerStats = transform.root.GetComponent<Entity>().Stats;
            _boostTimeLeft = _playerStats.MaxBoostTime;
        }

        void Update()
        {
            CanBoostToggle();
            ToggleBoostVFX();
            ConsumeBoost();
            RechargeBoost();
            UIEventsManager.Instance.UpdateBoostUI(_boostTimeLeft / _playerStats.MaxBoostTime);
        }

        void ToggleBoostVFX()
        {
            if (_canBoost && _isBoosting)
                _playerEventsManager.PlayerBoostVFXEvent(true);
            else
                _playerEventsManager.PlayerBoostVFXEvent(false);
        }

        void CanBoostToggle()
        {
            if (_boostTimeLeft > 0)
                _canBoost = true;
            else
                _canBoost = false;
        }

        private void ConsumeBoost()
        {
            if (_canBoost)
            {
                // If boosting, decrease boost time
                if (_isBoosting && _boostTimeLeft > 0)
                {
                    _boostTimeLeft -= Time.deltaTime;
                    _boostTimeLeft = Mathf.Max(_boostTimeLeft, 0f);
                }
            }
        }

        private void RechargeBoost()
        {
            if (!_isBoosting)
            {
                // If not boosting, recharge boost over time with the Stats recharge rate
                if (_boostTimeLeft < _playerStats.MaxBoostTime)
                {
                    _boostTimeLeft += _playerStats.BoostRechargeRate * Time.deltaTime;
                    _boostTimeLeft = Mathf.Min(_boostTimeLeft, _playerStats.MaxBoostTime);
                }
            }
        }

        // Callback from PlayerEventsManager, sets whether the player is boosting
        public void IsBoosting(bool value)
        {
            _isBoosting = value;
        }
    }
}
