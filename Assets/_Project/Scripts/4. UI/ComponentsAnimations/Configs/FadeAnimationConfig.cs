using System;
using UnityEngine;
using TriInspector;
using UnityEngine.UI;

namespace GoodVillageGames.Game.General.UI.Animations.Config
{
    [Serializable]
    public class FadeAnimationConfig : AnimationConfig
    {
        [field: SerializeField,  
                GUIColor(1.0f, 0.6f, 0.6f),
                LabelText("Object Image Component")] 
        public Image ComponentToAnimate { get; private set; }

        [field: SerializeField, 
                Range(0f, 1f), 
                GUIColor(0.4f, 0.8f, 1f),
                LabelText("Target Alpha"),
                PropertySpace(SpaceAfter = 10)]
        public float TargetAlpha { get; private set; } = 1f;
    }
}
