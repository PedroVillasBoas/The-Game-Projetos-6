using UnityEngine;
using GoodVillageGames.Game.Handlers;
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
        private BoostHandler _boostHandler;

        public Vector2 PlayerLinearVelocity { get => _playerRb.linearVelocity; }

        protected override void Awake()
        {
            base.Awake();
            PlayerEventsManager = GetComponentInChildren<PlayerEventsManager>();
            _playerRb = GetComponent<Rigidbody2D>();
            _boostHandler = GetComponentInChildren<BoostHandler>();
        }

        void FixedUpdate()
        {            
            if (!_boostHandler.IsUsingBoost)
                ProcessAcceleration();
            else
                ProcessBoostingAcceleration();
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
            Vector2 desiredVelocity = CalculateDesiredVelocity(Stats.MaxSpeed);
            ProcessAcceleration(desiredVelocity, Stats.Acceleration);
        }

        void ProcessBoostingAcceleration()
        {
            Vector2 desiredVelocity = CalculateDesiredVelocity(Stats.MaxBoostSpeed);
            ProcessAcceleration(desiredVelocity, Stats.MaxSpeed);
        }

        Vector2 CalculateDesiredVelocity(float speed)
        {
            Vector2 desiredVelocity = Vector2.zero;
            if (_movementInput.y > 0)
                desiredVelocity += (Vector2)transform.up;
            else if (_movementInput.y < 0)
                desiredVelocity -= (Vector2)transform.up;
            desiredVelocity += _movementInput.x * (Vector2)transform.right;
            return desiredVelocity * speed;
        }

        void ProcessAcceleration(Vector2 desiredVelocity, float speed)
        {
            _playerRb.linearVelocity = Vector2.MoveTowards(
                _playerRb.linearVelocity,
                desiredVelocity,
                speed * Time.fixedDeltaTime
            );

            PlayerEventsManager.PlayerMovingEvent(_playerRb.linearVelocity);
        }
    }
}
