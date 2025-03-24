using System;
using UnityEngine;
using TriInspector;
using System.Collections.Generic;
using static GoodVillageGames.Game.Enums.Enums;

namespace GoodVillageGames.Game.General.UI.Animations.Config
{
    [Serializable]
    public class AnimationTransitionConfig
    {
        [TableList(Draggable = true,
           HideAddButton = false,
           HideRemoveButton = false,
           AlwaysExpanded = false)]
        public List<AnimationTransitionConfig> AnimationTransitionConfigs;

        [Required]
        AnimationTransitionID CompletedAnimation;

        [Required]
        UIScreenID NextScreen;

        [Required]
        AnimationTransitionID NextAnimation;
    }
}
