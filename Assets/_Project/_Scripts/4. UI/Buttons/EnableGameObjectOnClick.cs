using GoodVillageGames.Game.Core.ScriptableObjects;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GoodVillageGames.Game.General.UI.Buttons
{
    public class EnableGameObjectOnClick : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private GameObject gameObjectToEnable;
        [SerializeField] private SceneScriptableObject sceneSO;

        public void OnPointerClick(PointerEventData eventData)
        {
            gameObjectToEnable.SetActive(true);

            foreach(TutorialButtonLogic button in gameObjectToEnable.GetComponentsInChildren<TutorialButtonLogic>())
                button.SceneNameSO = sceneSO;
        }
    }
}
