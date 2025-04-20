using GoodVillageGames.Game.Core.Global;
using UnityEngine;
using UnityEngine.EventSystems;
using static GoodVillageGames.Game.Enums.Enums;


namespace GoodVillageGames.Game.Handlers.UI
{
    public class UIChangerHandler : MonoBehaviour 
    { 
        private Animator animator;
        private GameState? currentState;

        void Awake()
        {
            animator = GetComponent<Animator>();
        }

        void Start()
        {
            GlobalEventsManager.Instance.ChangeGameStateEventTriggered += TriggerAnimation;
        }

        void OnDestroy()
        {
            GlobalEventsManager.Instance.ChangeGameStateEventTriggered -= TriggerAnimation;

        }

        void TriggerAnimation(GameState state)
        {
            // Prevent duplicate triggers
            if (currentState == state) return;

            currentState = state;
            animator.ResetTrigger("PAUSEtoGUI");

            switch (state)
            {
                case GameState.UpgradeScreen:
                    animator.SetTrigger("GUItoUPG");
                    ScenePauseHandler.Instance.PauseTimeScale();
                    break;
                    
                case GameState.GamePaused:
                    animator.SetTrigger("GUItoPAUSE");
                    break;

                case GameState.GameContinue:
                    animator.SetTrigger("PAUSEtoGUI");
                    EventSystem.current.SetSelectedGameObject(null); // Critical fix
                    break;
            }
        }

        // Add this to handle animation completion
        public void OnAnimationComplete()
        {
            currentState = null;
        }
    }
}
