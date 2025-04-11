using System.Collections;
using UnityEngine;

namespace GoodVillageGames.Game.Interfaces
{
    public interface IReloadHandler
    {
        bool IsReloading { get; set; }
        Coroutine ReloadCoroutine { get; set; }
        IEnumerator Reload();
        void CancelReload();
    }
}
