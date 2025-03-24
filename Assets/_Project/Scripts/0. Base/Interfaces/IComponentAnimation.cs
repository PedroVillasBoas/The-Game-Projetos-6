using DG.Tweening;
using System.Collections.Generic;
using static GoodVillageGames.Game.Enums.Enums;

namespace GoodVillageGames.Game.Interfaces
{
    public interface IComponentAnimation
    {
        void BuildAnimations();
        UIAnimationType GetAnimationType(UIAnimationType animationType);
        UIAnimationType GetAnimationType(AnimationTransitionID transitionID);
        UIAnimationType GetTransitionID(AnimationTransitionID transitionID);
        Dictionary<AnimationTransitionID, List<Tween>> Animations { get; }

    }
}
