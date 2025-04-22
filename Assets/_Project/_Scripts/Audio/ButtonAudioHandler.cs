using GoodVillageGames.Game.Core.Global;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GoodVillageGames.Game.Handlers.UI.Audio
{
    public class ButtonAudioHandler : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
    {
        public void OnPointerEnter(PointerEventData eventData)
        {
            GlobalAudioManager.Instance.PlayerOneShotSound(FMODEventsHandler.Instance.ButtonEnter, transform.position);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            GlobalAudioManager.Instance.PlayerOneShotSound(FMODEventsHandler.Instance.ButtonClick, transform.position);

        }
    }
}
