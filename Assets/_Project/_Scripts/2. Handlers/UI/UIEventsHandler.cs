using GoodVillageGames.Game.Core.Global;
using GoodVillageGames.Game.Core.Manager;
using UnityEngine;


namespace GoodVillageGames.Game.Handlers.UI
{
    public class UIEventsHandler : MonoBehaviour
    {
        public void TurnOffUIInput()
        {
            EventsManager.Instance.AnimationTriggerEvent(Enums.Enums.UIState.PLAYING_UI_ANIM);
        }

        public void TurnOnUIInput()
        {
            EventsManager.Instance.AnimationTriggerEvent(Enums.Enums.UIState.NORMAL_UI);
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

        public void StartGame()
        {
            EventsManager.Instance.GameStateTriggerEvent(Enums.Enums.GameState.InGame);
        }

        public void PauseGame()
        {
            EventsManager.Instance.GameStateTriggerEvent(Enums.Enums.GameState.Paused);
        }

        public void UpgradeTime()
        {
            EventsManager.Instance.GameStateTriggerEvent(Enums.Enums.GameState.UpgradeScreen);
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

        public void StartGameNow()
        {
            GlobalEventsManager.Instance.StartGame();
        }
    }
}
