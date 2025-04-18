using GoodVillageGames.Game.Core.Global;
using UnityEngine;
using static GoodVillageGames.Game.Enums.Enums;


namespace GoodVillageGames.Game.Handlers.UI
{
    public class UIChangerHandler : MonoBehaviour 
    { 
        private Animator animator;

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
            switch (state)
            {
                // case GameState.GameBegin:
                //     animator.SetTrigger("GUIDef");
                //     break;
                
                case GameState.UpgradeScreen:
                    animator.SetTrigger("GUItoUPG");
                    break;

                case GameState.GameContinue:
                    animator.SetTrigger("PAUSEtoGUI");
                    break;

                case GameState.GamePaused:
                    animator.SetTrigger("GUItoPAUSE");
                    break;
            }
        }
    }
}
