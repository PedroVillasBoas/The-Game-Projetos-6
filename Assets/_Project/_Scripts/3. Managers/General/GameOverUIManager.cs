using UnityEngine;
using GoodVillageGames.Game.Core.Global;
using static GoodVillageGames.Game.Enums.Enums;

namespace GoodVillageGames.Game.Core.Manager
{
    public class GameOverUIManager : MonoBehaviour 
    {
        private PoolManager poolManager;


        void Start()
        {
            GlobalEventsManager.Instance.ChangeGameState(GameState.GameOver);
        }
    }
}