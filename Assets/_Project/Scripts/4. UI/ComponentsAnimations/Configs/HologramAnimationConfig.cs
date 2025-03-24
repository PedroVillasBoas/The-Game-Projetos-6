using System;
using UnityEngine;
using TriInspector;
using UnityEngine.UI;

namespace GoodVillageGames.Game.General.UI.Animations.Config
{
    [Serializable]
    public class HologramAnimationConfig : AnimationConfig
    {
        [field: SerializeField, 
                GUIColor(0.4f, 0.8f, 1f),
                LabelText("Final Position")]
        public Vector2 FinalSize { get; private set; } = new(0f, 0f);

        [field: SerializeField, 
                Range(0.05f, 3f), 
                LabelText("Size Change Duration")]
        public float SizeChangeDuration { get; private set; } = 0.5f;

        [field: SerializeField,  
                GUIColor(1.0f, 0.6f, 0.6f),
                LabelText("Object RectTransform")] 
        public RectTransform ComponentToAnimate { get; private set; }

        [field: SerializeField]
        public bool EnableChildrenOnComplete { get; private set; } = false;

        [field: SerializeField]
        public bool Blink { get; private set; } = false;

        [field: SerializeField,  
                GUIColor(1.0f, 0.6f, 0.6f),
                LabelText("Object Image Component")] 
        public Image ComponentToAnimate2 { get; private set; }

        [field: ShowIf(nameof(Blink)),
                SerializeField, 
                Range(0f, 1f), 
                GUIColor(0.4f, 0.8f, 1f),
                LabelText("Target Alpha")]
        public float TargetAlpha { get; private set; } = 1f;

        [field: ShowIf(nameof(Blink)),
                SerializeField, 
                Range(1, 100), 
                GUIColor(0.4f, 0.8f, 1f),
                LabelText("Number of Blinks"),
                PropertySpace(SpaceAfter = 10)]
        public int BlinkLoops { get; private set; } = 1;
    }
}
