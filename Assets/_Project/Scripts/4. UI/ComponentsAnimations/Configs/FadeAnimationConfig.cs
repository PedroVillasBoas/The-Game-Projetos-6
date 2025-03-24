using System;
using UnityEngine;
using TriInspector;

namespace GoodVillageGames.Game.General.UI.Animations.Config
{
    [Serializable]
    public class FadeAnimationConfig : AnimationConfig
    {
        [field: SerializeField, 
                Range(0f, 1f), 
                GUIColor(0.4f, 0.8f, 1f),
                LabelText("Target Alpha")]
        public float TargetAlpha { get; private set; } = 1f;
    }
}
