using System;
using TriInspector;
using DG.Tweening;
using static GoodVillageGames.Game.Enums.Enums;

namespace GoodVillageGames.Game.General.UI
{
    [Serializable]
    public class SequenceActionType
    {
        [ShowInInspector]
        public UIButtonActionType UIButtomActionType { get; set; }
        public Sequence Sequence { get; }

        public SequenceActionType ( Sequence sequence)
        {
            Sequence = sequence;
        }
    }
}
