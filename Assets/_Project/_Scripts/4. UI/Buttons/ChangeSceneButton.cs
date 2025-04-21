using UnityEngine;
using UnityEngine.EventSystems;
using GoodVillageGames.Game.Core.Manager;
using GoodVillageGames.Game.Core.ScriptableObjects;

namespace GoodVillageGames.Game.General.UI.Buttons
{
    public class ChangeSceneButton : MonoBehaviour, IPointerClickHandler
    { 
        [SerializeField] private SceneScriptableObject sceneSO;

        public void OnPointerClick(PointerEventData eventData)
        {
            ChangeScene();
        }

        private void ChangeScene()
        {
            ChangeSceneManager.Instance.ChangeScene(sceneSO);
        }
    }
}
