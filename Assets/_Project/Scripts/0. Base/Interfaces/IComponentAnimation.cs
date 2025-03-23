using DG.Tweening;
using static GoodVillageGames.Game.Enums.Enums;

namespace GoodVillageGames.Game.Interfaces
{
    public interface IComponentAnimation
    {
        Tween ComponentTween { get; set; }
        float Duration { get; set; }
        AnimationID AnimationID { get; set; }
        UIAnimationType UIAnimationType { get; set; }
        void BuildAnimation();
        void AddComponentToSceneManagerStack();
    }
}
