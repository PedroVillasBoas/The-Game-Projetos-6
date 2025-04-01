using UnityEngine;

namespace GoodVillageGames.Game.Interfaces
{
    public interface IAimHandler
    {
        Vector3 TargetPosition { get; set; }
        Vector3 RotateDirection { get; set; }
        void HandleLook(Vector2 lookDirection);
        void Rotate();
    }
}
