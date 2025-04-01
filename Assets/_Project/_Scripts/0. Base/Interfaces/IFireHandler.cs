using UnityEngine;

namespace GoodVillageGames.Game.Interfaces
{
    public interface IFireHandler
    {
        IAimHandler AimHandler { get; set; }
        void Fire(GameObject prefab);
    }
}
