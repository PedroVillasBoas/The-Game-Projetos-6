using UnityEngine;

namespace GoodVillageGames.Game.General.UI.Buttons
{
    public class MainMenuQuitButton : MonoBehaviour
    {
        public void QuitGame()
        {
            // Adicionar o Popup de "Tem certeza, jogador?"
            Application.Quit();
        }
    }
}
