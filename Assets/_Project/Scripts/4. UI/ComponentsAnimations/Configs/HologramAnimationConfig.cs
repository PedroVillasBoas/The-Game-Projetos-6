using System;
using UnityEngine;
using TriInspector;

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

        [field: SerializeField]
        public bool EnableChildrenOnComplete { get; private set; } = false;

        [field: SerializeField]
        public bool Blink { get; private set; } = false;

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
                LabelText("Number of Blinks")]
        public int BlinkLoops { get; private set; } = 1;
    }
}
