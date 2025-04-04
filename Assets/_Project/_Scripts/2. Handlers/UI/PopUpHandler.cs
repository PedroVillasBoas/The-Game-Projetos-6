using GoodVillageGames.Game.Core.Manager;
using UnityEngine;

namespace GoodVillageGames.Game.Handlers.UI
{
    public class PopUpHandler : MonoBehaviour
    {
        [SerializeField] private GameObject _content;

        void Start()
        {
            EventsManager.Instance.OnEventTriggered += HandlePopUp;
        }

        void OnDisable()
        {
            EventsManager.Instance.OnEventTriggered -= HandlePopUp;
        }

        void HandlePopUp(string value)
        {
            switch (value)
            {
                case "CloseGamePopUp":
                    _content.SetActive(false);
                    break;
                
                case "OpenGamePopUp":
                    _content.SetActive(true);
                    break;
                
                case "QuitGamePopUp":
                    // handle the quit game here, and wait for the Game Manager to give the signal that can change scene
                        // Then, change scene
                    break;

                default:
                    Debug.LogError("Value Unknown to PopUp Handler");
                    break;
            }
        }
    }
}
