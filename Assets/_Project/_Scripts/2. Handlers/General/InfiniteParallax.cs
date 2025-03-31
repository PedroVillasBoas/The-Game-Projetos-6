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

            _size = GetComponentInChildren<SpriteRenderer>().bounds.size;
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();

            InfiniteBackground();
        }

        public void InfiniteBackground()
        {
            _movement = MainCamera.transform.position * ( 1 - ParallaxEffect);

            if (Utillities.Vector2IsGreaterThan(_movement, StartPosition + Size))
                StartPosition += Size;
            else if (Utillities.Vector2IsLessThan(_movement, StartPosition - Size))
                StartPosition -= Size;
        }
    }
}
