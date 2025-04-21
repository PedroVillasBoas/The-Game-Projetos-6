using UnityEngine;
using GoodVillageGames.Game.Interfaces;
using GoodVillageGames.Game.Core.Manager;
using GoodVillageGames.Game.Core.ScriptableObjects;

namespace GoodVillageGames.Game.General.UI.Buttons
{
    public class ButtonLogic : MonoBehaviour, IButtonAction
    {
        [SerializeField] private SceneScriptableObject _sceneName;

        public SceneScriptableObject SceneNameSO { get => _sceneName; set => _sceneName = value; }

        public void CallChangeScene()
        {
            ChangeSceneManager.Instance.ChangeScene(_sceneName);
        }
    }
}
