using GoodVillageGames.Game.Interfaces;
using UnityEngine;

namespace GoodVillageGames.Game.Core.Util
{
    public class DestroyOnTrigger : MonoBehaviour, ITrigger
    {
        public void OnTrigger()
        {
            Destroy(gameObject);
        }
    }
}
