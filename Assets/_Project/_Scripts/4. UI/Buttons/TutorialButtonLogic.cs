using GoodVillageGames.Game.Core.Global;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GoodVillageGames.Game.General.UI.Buttons
{
    public class TutorialButtonLogic : ButtonLogic, IPointerClickHandler
    {
        [SerializeField] private bool playTutorial = true;

        public void OnPointerClick(PointerEventData eventData)
        {
            GlobalEventsManager.Instance.ChangeTutorialChoice(playTutorial);
            ButtonAction();
        }
    }
}
