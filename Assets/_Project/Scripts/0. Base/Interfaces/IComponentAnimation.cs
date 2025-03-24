using DG.Tweening;
using System.Collections.Generic;
using static GoodVillageGames.Game.Enums.Enums;

namespace GoodVillageGames.Game.Interfaces
{
    public interface IComponentAnimation
    {
        void BuildAnimations();
        UIAnimationType GetAnimationType(AnimationTransitionID transitionID);
        AnimationTransitionID GetTransitionID(UIAnimationType animationType);
        Dictionary<AnimationTransitionID, List<Tween>> Animations { get; }

    }
}
