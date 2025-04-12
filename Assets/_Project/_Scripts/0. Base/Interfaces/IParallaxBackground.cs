using Unity.Cinemachine;
using UnityEngine;

namespace GoodVillageGames.Game.Interfaces
{
    public interface IParallaxBackground
    {
        Vector2 StartPosition { get; set; }
        Vector2 Distance { get; set; }
        CinemachineVirtualCameraBase MainCamera { get; set; }
        float ParallaxEffect { get; set; }

        void ParallaxBackground();
    }
}
