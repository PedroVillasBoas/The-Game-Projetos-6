using UnityEngine;
using GoodVillageGames.Game.Core.Global;
using GoodVillageGames.Game.Core.Manager;
using static GoodVillageGames.Game.Enums.Enums;


namespace GoodVillageGames.Game.Handlers.UI
{
    public partial class UIEventsHandler : MonoBehaviour
    {
        [SerializeField] private ScenePauseHandler scenePauseHandler;

        public void PlayingUIAnimation()
        {
            GlobalEventsManager.Instance.AnimationTriggerEvent(UIState.PLAYING_UI_ANIM);
        }

        public void LoopingUIAnimation()
        {
            GlobalEventsManager.Instance.AnimationTriggerEvent(UIState.NORMAL_UI);
        }

        public void TurnOnPlayerInput()
        {
            GlobalEventsManager.Instance.AnimationTriggerEvent(UIState.NoAnimationPlaying);
            scenePauseHandler.ReturnToOriginalTimeScale();
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
