using System;
using UnityEngine;
using TriInspector;

namespace GoodVillageGames.Game.General.UI.Animations.Config
{
    [Serializable]
    public class MoveAnimationConfig : AnimationConfig
    {
        [field: SerializeField,  
                GUIColor(1.0f, 0.6f, 0.6f),
                LabelText("Object RectTransform")] 
        public RectTransform ComponentToAnimate { get; private set; }

        [field: SerializeField, 
                GUIColor(0.4f, 0.8f, 1f),
                LabelText("Final Position"),
                PropertySpace(SpaceAfter = 10)]
        public Vector2 FinalPosition { get; private set; } = new(0f, 0f);
    }
}
