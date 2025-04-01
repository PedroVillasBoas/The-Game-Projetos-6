using GoodVillageGames.Game.Interfaces;
using UnityEngine;

namespace GoodVillageGames.Game.Handlers
{
    public class PlayerAimHandler : MonoBehaviour, IAimHandler
    {
        // Local
        private Vector3 _mouseWorldPosition;
        private Vector3 _rotateDiretion;

        public Vector3 TargetPosition { get => _mouseWorldPosition; set => _mouseWorldPosition = value; }
        public Vector3 RotateDirection { get => _rotateDiretion; set => _rotateDiretion = value; }

        void Update()
        {
            Rotate();
        }

        public void HandleLook(Vector2 lookDirection)
        {
            _mouseWorldPosition = Camera.main.ScreenToWorldPoint(lookDirection);
            _mouseWorldPosition.z = 0f;
        }

        public void Rotate()
        {
            _rotateDiretion = new Vector2(_mouseWorldPosition.x - transform.position.x, _mouseWorldPosition.y - transform.position.y);

            transform.up = _rotateDiretion;
        }
    }
}
