using UnityEngine;

namespace GoodVillageGames.Game.Handlers
{
    public class PlayerAimHandler : MonoBehaviour
    {
        private Camera _mainCamera;
        private Vector3 _mouseWorldPosition;

        private Vector3 _rotateDiretion;

        void Awake()
        {
            _mainCamera = Camera.main;
        }

        void Update()
        {
            RotatePlayer();
        }

        public void HandleLook(Vector2 input)
        {
            _mouseWorldPosition = Camera.main.ScreenToWorldPoint(input);
            _mouseWorldPosition.z = 0f;
        }

        void RotatePlayer()
        {
            _rotateDiretion = new Vector2(_mouseWorldPosition.x - transform.position.x, _mouseWorldPosition.y - transform.position.y);

            transform.up = _rotateDiretion;
        }
    }
}
