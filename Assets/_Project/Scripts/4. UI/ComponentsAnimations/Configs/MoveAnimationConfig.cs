using System;
using UnityEngine;
using TriInspector;

namespace GoodVillageGames.Game.General.UI.Animations.Config
{
    [Serializable]
    public class MoveAnimationConfig : AnimationConfig
    {
        [field: SerializeField, 
                GUIColor(0.4f, 0.8f, 1f),
                LabelText("Final Position")]
        public Vector2 FinalPosition { get; private set; } = new(0f, 0f);
    }
}
