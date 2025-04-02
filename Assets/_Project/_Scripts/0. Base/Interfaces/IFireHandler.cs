using UnityEngine;
using GoodVillageGames.Game.Handlers;

namespace GoodVillageGames.Game.Interfaces
{
    public interface IFireHandler
    {
        IAimHandler AimHandler { get; set; }
        ReloadHandler ReloadHandler { get; set; }
        Transform Firepoint { get; set; }
        GameObject ProjectilePrefab { get; set; }
        Coroutine FireCoroutine { get; set; }
        void FireProjectile();
    }
}
