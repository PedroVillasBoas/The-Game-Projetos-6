using GoodVillageGames.Game.Core.Global;
using GoodVillageGames.Game.Core.Manager;
using UnityEngine;
using static GoodVillageGames.Game.Enums.Enums;


namespace GoodVillageGames.Game.Handlers.UI
{
    public partial class UIEventsHandler : MonoBehaviour
    {
        public void TurnOffUIInput()
        {
            EventsManager.Instance.AnimationTriggerEvent(UIState.PLAYING_UI_ANIM);
        }

        public void TurnOnUIInput()
        {
            EventsManager.Instance.AnimationTriggerEvent(UIState.NORMAL_UI);
        }

        public void TurnOnMoveStars()
        {
            EventsManager.Instance.TriggerEvent("Move");
        }

        public void TurnOnMoveBackStars()
        {
            EventsManager.Instance.TriggerEvent("MoveBack");
        }

        public void TurnOffMoveStars()
        {
            EventsManager.Instance.TriggerEvent("Stop");
        }

        public void StartTutorial()
        {
            GlobalEventsManager.Instance.ChangeGameState(GameState.Tutorial);
        }

        public void StartGame()
        {
            GlobalEventsManager.Instance.ChangeGameState(GameState.GameBegin);
        }

        public void OpenQuitGamePopUp()
        {
            EventsManager.Instance.TriggerEvent("OpenGamePopUp");
        }

        public void QuitGamePopUp()
        {
            EventsManager.Instance.TriggerEvent("QuitGamePopUp");
        }

        public void CloseGamePopUp()
        {
            EventsManager.Instance.TriggerEvent("CloseGamePopUp");
        }
    }
}
