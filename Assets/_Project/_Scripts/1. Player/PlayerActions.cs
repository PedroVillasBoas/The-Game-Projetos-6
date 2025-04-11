using UnityEngine;
using TriInspector;
using GoodVillageGames.Game.Core.Manager.Player;
using GoodVillageGames.Game.Core.GameObjectEntity;

namespace GoodVillageGames.Game.Core
{
    public class PlayerActions : Entity
    {
        // Public
        [HideInInspector] public PlayerEventsManager PlayerEventsManager;

        // Local
        private Rigidbody2D _playerRb;
        private Vector2 _movementInput = Vector2.zero;

        protected override void Awake()
        {
            base.Awake();
            PlayerEventsManager = GetComponentInChildren<PlayerEventsManager>();
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
            PlayerEventsManager.PlayerBulletEvent(value);
        }
        public void HandleBoost(bool value)
        {
            // I'll have to create a new handler to handle the boost and boost amount
            PlayerEventsManager.PlayerBoostingEvent(value);
        }
        public void HandleMissile(bool value)
        {
            PlayerEventsManager.PlayerMissileEvent(value);
        }

        void ProcessAcceleration()
        {
            Vector2 desiredVelocity = Vector2.zero;

            if (_movementInput.y > 0)
                desiredVelocity += (Vector2)transform.up * Stats.MaxSpeed;
            else if (_movementInput.y < 0)
                desiredVelocity -= (Vector2)transform.up * Stats.MaxSpeed;

            desiredVelocity += _movementInput.x * Stats.MaxSpeed * (Vector2)transform.right;

            _playerRb.linearVelocity = Vector2.MoveTowards(
                _playerRb.linearVelocity,
                desiredVelocity,
                Stats.Acceleration * Time.fixedDeltaTime
            );

            PlayerEventsManager.PlayerMovingEvent(_playerRb.linearVelocity);
        }

    }
}
