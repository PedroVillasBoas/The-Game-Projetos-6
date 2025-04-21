using UnityEngine;

namespace GoodVillageGames.Game.General.UI.Buttons
{
    public class ChangePlayerButton : MonoBehaviour 
    { 
        [SerializeField] private GameObject changePlayerUI;

        public void EnableChangePlayer()
        {
            changePlayerUI.SetActive(true);
        }
    }
}