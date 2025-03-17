using DG.Tweening;
using GoodVillageGames.Game.Core.Manager;
using UnityEngine;
using UnityEngine.Events;

namespace GoodVillageGames.Game.Core
{
    public class PlayerActions : MonoBehaviour
    {
        private PlayerStatsManager _playerStatsManager;

        private Rigidbody2D _playerRb;

        private Vector2 _movementInput = Vector2.zero;
        private Vector2 _targetVelocity = Vector2.zero;

        private Tweener _movementTweener;

        // Events
        public UnityEvent<Vector2> onPlayerMovingEvent;
        public UnityEvent<bool> onPlayerBoostingEvent;

        void Awake()
        {
            _playerStatsManager = GetComponentInChildren<PlayerStatsManager>();
            _playerRb = GetComponent<Rigidbody2D>();
        }

        void FixedUpdate()
        {
            ProcessMovement();
        }

        public void HandleMove(Vector2 input)
        {
            _movementInput = input;
        }

        public void HandleAttack(bool value)
        {
            // Here I'll just implement the check logic -> pressing the button
            // Later I'll implement the Coroutine to handle the pew pew pew in auto/single mode based on the attackspeed
        }
        public void HandleBoost(bool value)
        {
            // Here I'll put the boosting check
            // I'll have to create a new handler to handle the boost and boost amount
            onPlayerBoostingEvent?.Invoke(value);
        }
        public void HandleMissile(bool value)
        {
            // Here I'll just implement the check logic -> see if has missile avaliable
            // Later I'll implement the Coroutine to handle the pew BOOM pew BOOM pew BOOM in auto/single mode based on the missile attack speed
        }

        void ProcessMovement()
        {
            ProcessAcceleration();
        }

        void ProcessAcceleration()
        {
            _targetVelocity = _movementInput.y > 0 ? transform.up * _playerStatsManager.MaxSpeed : Vector2.zero;
            _targetVelocity += _movementInput.y < 0 ? -transform.up * _playerStatsManager.MaxSpeed : Vector2.zero;
            _targetVelocity += _movementInput.x * _playerStatsManager.MaxSpeed * (Vector2)transform.right;

            Debug.Log(_targetVelocity);

            if (_movementTweener != null && _movementTweener.IsActive())
            {
                _movementTweener.Kill();
            }

            float duration = Vector2.Distance(_playerRb.linearVelocity, _targetVelocity) / _playerStatsManager.Acceleration;
            duration = Mathf.Max(duration, 0.01f);

            _movementTweener = DOTween.To(
            () => _playerRb.linearVelocity,
            x => _playerRb.linearVelocity = x,
            _targetVelocity,
            duration
            ).SetEase(Ease.Linear).OnUpdate(() => onPlayerMovingEvent?.Invoke(_playerRb.linearVelocity));
        }

    }
}
