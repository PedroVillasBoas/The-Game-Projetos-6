using System;
using UnityEngine;
using TriInspector;

namespace GoodVillageGames.Game.General.UI.Animations.Config
{
    [Serializable]
    public class ZoomAnimationConfig : AnimationConfig
    {
        [field: SerializeField,
                GUIColor(0.4f, 0.8f, 1f),
                LabelText("Final Size")]
        public Vector2 FinalSize { get; private set; } = new(0f, 0f);
    }
}
