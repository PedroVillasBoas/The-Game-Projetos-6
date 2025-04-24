using GoodVillageGames.Game.Handlers.UI;
using UnityEngine;

namespace GoodVillageGames.Game.Core.Util
{
    public class AskForUnPause : MonoBehaviour 
    { 
        public void AskUnpause() => ScenePauseHandler.Instance.ReturnToOriginalTimeScale();
    }
}
