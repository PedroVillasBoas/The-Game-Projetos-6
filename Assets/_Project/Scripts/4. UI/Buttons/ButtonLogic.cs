using UnityEngine;
using TriInspector;
using GoodVillageGames.Game.Interfaces;
using GoodVillageGames.Game.Core.Manager;
using static GoodVillageGames.Game.Enums.Enums;
using GoodVillageGames.Game.Core.ScriptableObjects;

namespace GoodVillageGames.Game.General.UI.Buttons
{
    public class ButtonLogic : MonoBehaviour, IButtonAction
    {
        [SerializeField] private AnimationTransitionID _animationTransitionID;
        [SerializeField] private SequenceActionType _sequenceActionType;

        [PropertyTooltip("Move or MoveBack")]
        [SerializeField] private string _starsMovementType;

        public AnimationTransitionID AnimationTransitionID { get => _animationTransitionID; set => _animationTransitionID = value; }
        public SequenceActionType SequenceActionType { get => _sequenceActionType; set => _sequenceActionType = value; }

        public void ButtomAction()
        {
            EventsManager.Instance.ButtonAskingAnimationEventTriggered(_animationTransitionID);
            EventsManager.Instance.TriggerEvent(_starsMovementType);
        }
    }
}
