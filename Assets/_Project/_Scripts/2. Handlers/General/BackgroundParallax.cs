using UnityEngine;
using GoodVillageGames.Game.Interfaces;

namespace GoodVillageGames.Game.Handlers
{
    public class BackgroundParallax : MonoBehaviour, IParallaxBackground
    {
        private Vector2 _startPosition;
        private Vector2 _distance;
        private Camera _mainCamera;

        [SerializeField] private float _parallaxeffect;

        public Vector2 StartPosition { get => _startPosition; set => _startPosition = value; }
        public Vector2 Distance { get => _distance; set => _distance = value; }
        public Camera MainCamera { get => _mainCamera; set => _mainCamera = value; }
        public float ParallaxEffect { get => _parallaxeffect; set => _parallaxeffect = value; }

        void Awake()
        {
            MainCamera = Camera.main;
        }

        protected virtual void Start()
        {
            _startPosition = new Vector2(transform.position.x, transform.position.y);
        }

        protected virtual void FixedUpdate()
        {
            ParallaxBackground();
        }

        public void ParallaxBackground()
        {
            _distance = new Vector2(_mainCamera.transform.position.x, _mainCamera.transform.position.y) * _parallaxeffect;

            transform.position = new Vector3(_startPosition.x + _distance.x, _startPosition.y + _distance.y, transform.position.z);
        }
    }
}
