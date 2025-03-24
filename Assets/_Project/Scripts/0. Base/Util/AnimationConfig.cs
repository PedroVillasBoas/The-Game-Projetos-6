using System;
using UnityEngine;
using DG.Tweening;
using TriInspector;
using static GoodVillageGames.Game.Enums.Enums;

namespace GoodVillageGames.Game.General.UI.Animations.Config
{
    [Serializable]
    public class AnimationConfig
    {
        [Title("Animation Configuration")]
        [field: SerializeField] 
        public AnimationTransitionID Animation { get; private set; }

        [field: SerializeField] 
        public AnimationTransitionID AnimationTransitionID { get; private set; }

        [field: SerializeField, 
                GUIColor(1f, 1f, 0f)] 
        public UIAnimationType UIAnimationType { get; private set; }

        [field: SerializeField, 
                Range(0.05f, 3f), 
                GUIColor(0.8f, 1.0f, 0.6f)] 
        public float Duration { get; private set; } = 0.5f;

        [field: SerializeField,  
                GUIColor(1.0f, 0.6f, 0.6f)] 
        public Component ComponentToAnimate { get; private set; }
    }

}
