using UnityEngine;
using GoodVillageGames.Game.Core.Global;

namespace GoodVillageGames.Game.Core.Util
{
    public class FirstLoginEnableOnTrigger : EnableObjectOnTrigger 
    {
        public override bool ConditionToTrigger()
        {
            return GlobalGameManager.Instance.FirstLogin;
        }
    }
}
