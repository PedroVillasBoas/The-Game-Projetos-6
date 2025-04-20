using UnityEngine;
using UnityEngine.EventSystems;

namespace GoodVillageGames.Game.General.UI.Buttons
{
    public class ButtonAnimationLogic : MonoBehaviour, IPointerClickHandler
    { 
        [SerializeField] private Animator animator;
        [SerializeField] private string trigger;

        public void OnPointerClick(PointerEventData eventData)
        {
            animator.SetTrigger(trigger);
        }
    }
}
