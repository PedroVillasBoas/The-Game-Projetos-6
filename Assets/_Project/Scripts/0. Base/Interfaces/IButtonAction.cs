using static GoodVillageGames.Game.Enums.Enums;
using GoodVillageGames.Game.Core.ScriptableObjects;
using GoodVillageGames.Game.General.UI;

namespace GoodVillageGames.Game.Interfaces
{
    public interface IButtonAction
    {
        SequenceActionType SequenceActionType { get; set; }
        AnimationID AnimationID { get; set; }
        SceneScriptableObject SceneSO { get; set; }
        void ButtomAction();
        void SendButtonActionToSceneManager();
    }
}
