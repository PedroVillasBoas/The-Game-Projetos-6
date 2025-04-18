using GoodVillageGames.Game.Core.Attributes.Modifiers;
using GoodVillageGames.Game.Core.Manager;
using UnityEngine;


namespace GoodVillageGames.Game.Handlers.UI
{
    public class SceneUpgradeHandler : MonoBehaviour 
    { 


        public void OnChoiceSelected(UpgradeStatModifier upgrade)
        {
            // take the upgrade from the card choice
            // take the stat modifier from the upgrade
            // apply on the player
            // End upgrade time

            PlayerUpgraderManager.Instance.AddUpgradeToPlayer(upgrade);
        }
    }
}
