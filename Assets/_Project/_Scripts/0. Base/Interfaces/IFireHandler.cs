using UnityEngine;
using GoodVillageGames.Game.Enums.Pooling;

namespace GoodVillageGames.Game.Interfaces
{
    public interface IFireHandler
    {
        IAimHandler AimHandler { get; set; }
        IReloadHandler ReloadHandler { get; set; }
        Transform Firepoint { get; set; }
        PoolID ProjectilePoolID { get; set; }
        Coroutine FireCoroutine { get; set; }
        void FireProjectile();
    }
}
