using GoodVillageGames.Game.Handlers.UI;
using UnityEngine.EventSystems;

namespace GoodVillageGames.Game.General.UI.Buttons
{
    public class BackToMainMenuButton : ChangeSceneButton 
    {
        public override void OnPointerClick(PointerEventData eventData)
        {
            ScenePauseHandler.Instance.ReturnToOriginalTimeScale();
            base.OnPointerClick(eventData);
        }
    }
}
