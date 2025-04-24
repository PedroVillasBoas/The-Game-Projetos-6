using UnityEngine;
using GoodVillageGames.Game.Core.Global;
using static GoodVillageGames.Game.Enums.Enums;

namespace GoodVillageGames.Game.Core.Util
{
    public class ChangeGameStateTrigger : MonoBehaviour 
    { 
        [SerializeField] private GameState gameState;
        [SerializeField] private bool triggerOnAnimation = false;

        void Start()
        {
            if(!triggerOnAnimation)
                GlobalEventsManager.Instance.ChangeGameState(gameState);
        }

        public void GameStateTriggerOnAnimation()
        {
            GlobalEventsManager.Instance.ChangeGameState(gameState);
        }
    }
}
