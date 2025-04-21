using GoodVillageGames.Game.General.UI.Buttons;
using GoodVillageGames.Game.Interfaces;
using UnityEngine;

namespace GoodVillageGames.Game.Core.Util
{
    public class DisableObjectOnEventTrigger : MonoBehaviour
    {
        [SerializeField] private FirstLoginButton button;

        void OnEnable()
        {
            button.PlayerNameSetEventTriggered += DisableSelf;
            
        }

        void DisableSelf()
        {
            gameObject.SetActive(false);
        }
    }
}
