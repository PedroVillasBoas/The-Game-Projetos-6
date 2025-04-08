using GoodVillageGames.Game.Interfaces;
using UnityEngine;


namespace GoodVillageGames.Game.Handlers
{
    public class MissileFireHandler : MonoBehaviour, IFireHandler
    {
        public IAimHandler AimHandler { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public ReloadHandler ReloadHandler { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public Transform Firepoint { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public Enums.Enums.PoolID ProjectilePoolID { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public Coroutine FireCoroutine { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public void FireProjectile()
        {
            throw new System.NotImplementedException();
        }
    }
}
