using DG.Tweening;
using GoodVillageGames.Game.Core.Manager;
using UnityEngine;
using UnityEngine.Events;

namespace GoodVillageGames.Game.Core
{
    public class PlayerActions : MonoBehaviour
    {
        

        // Local
        private PlayerStatsManager _playerStatsManager;
        private Rigidbody2D _playerRb;
        private Vector2 _movementInput = Vector2.zero;

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
            ProcessAcceleration();
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

        void ProcessAcceleration()
        {
            Vector2 desiredVelocity = Vector2.zero;

            if (_movementInput.y > 0)
                desiredVelocity += (Vector2)transform.up * _playerStatsManager.MaxSpeed;
            else if (_movementInput.y < 0)
                desiredVelocity -= (Vector2)transform.up * _playerStatsManager.MaxSpeed;

            desiredVelocity += _movementInput.x * _playerStatsManager.MaxSpeed * (Vector2)transform.right;

            _playerRb.linearVelocity = Vector2.MoveTowards(
                _playerRb.linearVelocity,
                desiredVelocity,
                _playerStatsManager.Acceleration * Time.fixedDeltaTime
            );

            onPlayerMovingEvent?.Invoke(_playerRb.linearVelocity);
        }

    }
}
