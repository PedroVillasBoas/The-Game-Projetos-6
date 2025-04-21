using GoodVillageGames.Game.Interfaces;
using UnityEngine;

namespace GoodVillageGames.Game.Core.Util
{
    public class EnableObjectOnTrigger : MonoBehaviour, ITrigger
    {
        [SerializeField] private GameObject objectToEnable;

        public void OnTrigger()
        {
            if (ConditionToTrigger())
                objectToEnable.SetActive(true);
        }

        public virtual bool ConditionToTrigger()
        {
            return default;
        }
    }
}
