using GoodVillageGames.Game.Handlers.UI;
using UnityEngine;

namespace GoodVillageGames.Game.General.UI.Buttons
{
    public class BeginGameButton : MonoBehaviour 
    { 
        public void LeaveTutorialAndBeginGame()
        {
            UIEventsHandler temp = FindFirstObjectByType<UIEventsHandler>();
            temp.TurnOnPlayerInput();
        }
    }
}
