using GoodVillageGames.Game.Core.ScriptableObjects;

namespace GoodVillageGames.Game.Interfaces
{
    public interface IButtonAction
    {
        SceneScriptableObject SceneNameSO { get; set; }
        void CallChangeScene();
    }
}
