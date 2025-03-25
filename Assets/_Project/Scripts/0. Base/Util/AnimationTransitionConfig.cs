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
        [field: SerializeField,  
                LabelText("Completed Animation")] 
        public AnimationTransitionID CompletedAnimation { get; private set; }

        [field: SerializeField,  
                LabelText("Enable?")] 
        public bool ChildrenValue { get; private set; }

        [field: SerializeField,
                LabelText("Components To Toggle")] 
        public List<GameObject> ObjectToToggle { get; private set; }

    }
}
