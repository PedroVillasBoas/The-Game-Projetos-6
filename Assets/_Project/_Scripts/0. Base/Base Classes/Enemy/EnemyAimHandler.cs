using GoodVillageGames.Game.Interfaces;
using UnityEngine;

namespace GoodVillageGames.Game.Core.Enemy
{
    public class EnemyAimHandler : MonoBehaviour, IAimHandler
    {
        // Local
        private Vector3 _playerPosition;
        private Vector3 _rotateDiretion;

        public Vector3 TargetPosition { get => _playerPosition; set => _playerPosition = value; }
        public Vector3 RotateDirection { get => _rotateDiretion; set => _rotateDiretion = value; }

        void Update()
        {
            Rotate();
        }

        public void HandleLook(Vector2 lookDirection)
        {
            _playerPosition =  new(lookDirection.x, lookDirection.y, 0f);
        }

        public void Rotate()
        {
            _rotateDiretion = _playerPosition - transform.position;
            transform.up = _rotateDiretion;
        }
    }
}