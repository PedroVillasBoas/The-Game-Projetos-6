using static GoodVillageGames.Game.Enums.Enums;
using GoodVillageGames.Game.Core.ScriptableObjects;
using GoodVillageGames.Game.General.UI;

namespace GoodVillageGames.Game.Interfaces
{
    public interface IButtonAction
    {
        SequenceActionType SequenceActionType { get; set; }
        AnimationTransitionID AnimationTransitionID { get; set; }
        void ButtomAction();
    }
}
