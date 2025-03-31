using UnityEngine;

namespace GoodVillageGames.Game.Interfaces
{
    public interface IInfiniteBackground
    {
        Vector2 Size { get; set; }
        Vector2 Movement { get; set; }

        void InfiniteBackground();
    }
}
