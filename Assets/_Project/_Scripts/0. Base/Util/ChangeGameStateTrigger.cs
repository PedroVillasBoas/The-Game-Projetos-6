using GoodVillageGames.Game.Core.Global;
using UnityEngine;
using static GoodVillageGames.Game.Enums.Enums;

namespace GoodVillageGames.Game.Core.Util
{
    public class ChangeGameStateTrigger : MonoBehaviour 
    { 
        [SerializeField] private GameState gameState;


        void Start() => GlobalEventsManager.Instance.ChangeGameState(gameState);
    }
}
