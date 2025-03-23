using UnityEngine;
using GoodVillageGames.Game.Interfaces;
using static GoodVillageGames.Game.Enums.Enums;
using GoodVillageGames.Game.Core.ScriptableObjects;
using GoodVillageGames.Game.Core.Manager;

namespace GoodVillageGames.Game.General.UI.Buttons
{
    public class SplashScreenButton : MonoBehaviour, IButtonAction
    {
        [SerializeField] private AnimationID _animationID;
        [SerializeField] private SceneScriptableObject _scene;
        [SerializeField] private SequenceActionType _sequenceActionType;

        public AnimationID AnimationID { get => _animationID; set => _animationID = value; }
        public SceneScriptableObject SceneSO { get => _scene; set => _scene = value; }
        public SequenceActionType SequenceActionType { get => _sequenceActionType; set => _sequenceActionType = value; }

        public void SendButtonActionToSceneManager()
        {
            EventsManager.Instance.AddComponentToStackTriggered(gameObject);
        }

        public void ButtomAction()
        {
            EventsManager.Instance.ButtonAnimationEventTriggered(_animationID, _scene);
            EventsManager.Instance.TriggerEvent("Move");
        }
    }
}
