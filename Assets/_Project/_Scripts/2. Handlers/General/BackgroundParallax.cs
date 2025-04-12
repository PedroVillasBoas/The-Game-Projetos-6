using UnityEngine;
using GoodVillageGames.Game.Interfaces;
using Unity.Cinemachine;

namespace GoodVillageGames.Game.Handlers
{
    public class BackgroundParallax : MonoBehaviour, IParallaxBackground
    {
        private Vector2 _startPosition;
        private Vector2 _distance;

        [SerializeField] private Camera _mainCamera;
        [SerializeField] private float _parallaxEffect;

        public Vector2 StartPosition { get => _startPosition; set => _startPosition = value; }
        public Vector2 Distance { get => _distance; set => _distance = value; }
        public Camera MainCamera { get => _mainCamera; set => _mainCamera = value; }
        public float ParallaxEffect { get => _parallaxEffect; set => _parallaxEffect = value; }

        protected virtual void Start()
        {
            _startPosition = transform.position;
        }

        protected virtual void Update()
        {
            ParallaxBackground();
        }

        public void ParallaxBackground()
        {
            _distance = new Vector2(_mainCamera.transform.position.x, _mainCamera.transform.position.y) * _parallaxEffect;
            transform.position = new Vector3(_startPosition.x + _distance.x, _startPosition.y + _distance.y, transform.position.z);
        }
    }
}
