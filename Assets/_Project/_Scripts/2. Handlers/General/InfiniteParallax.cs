using UnityEngine;
using GoodVillageGames.Game.Interfaces;
using GoodVillageGames.Game.Core.Util;

namespace GoodVillageGames.Game.Handlers
{
    public class InfiniteParallax : BackgroundParallax, IInfiniteBackground
    {
        private Vector2 _size;
        private Vector2 _movement;

        public Vector2 Size { get => _size; set => _size = value; }
        public Vector2 Movement { get => _movement; set => _movement = value; }

        protected override void Start()
        {
            base.Start();

            SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
            if (sr != null)
            {
                _size = sr.bounds.size;
            }
            else
            {
                Debug.LogWarning("InfiniteParallax: Nenhum SpriteRenderer encontrado nos filhos para determinar o tamanho do background.");
            }
        }

        protected override void Update()
        {
            base.Update();
            InfiniteBackground();
        }

        public void InfiniteBackground()
        {
            _movement = MainCamera.transform.position * (1 - ParallaxEffect);

            while (_movement.x > StartPosition.x + Size.x)
            {
                StartPosition = new Vector2(StartPosition.x + Size.x, StartPosition.y);
            }
            while (_movement.x < StartPosition.x - Size.x)
            {
                StartPosition = new Vector2(StartPosition.x - Size.x, StartPosition.y);
            }

            while (_movement.y > StartPosition.y + Size.y)
            {
                StartPosition = new Vector2(StartPosition.x, StartPosition.y + Size.y);
            }
            while (_movement.y < StartPosition.y - Size.y)
            {
                StartPosition = new Vector2(StartPosition.x, StartPosition.y - Size.y);
            }
        }
    }
}
