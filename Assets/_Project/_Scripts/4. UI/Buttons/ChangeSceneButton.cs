using UnityEngine;
using UnityEngine.EventSystems;
using GoodVillageGames.Game.Core.Manager;
using GoodVillageGames.Game.Core.ScriptableObjects;

namespace GoodVillageGames.Game.General.UI.Buttons
{
    public class ChangeSceneButton : MonoBehaviour, IPointerClickHandler
    { 
        [SerializeField] protected SceneScriptableObject sceneSO;

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            ChangeSceneManager.Instance.ChangeScene(sceneSO);
        }
    }
}
