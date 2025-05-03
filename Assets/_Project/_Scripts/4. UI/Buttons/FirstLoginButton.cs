using TMPro;
using System;
using UnityEngine;
using System.IO;
using GoodVillageGames.Game.Core.Global;

namespace GoodVillageGames.Game.General.UI.Buttons
{
    public class FirstLoginButton : MonoBehaviour
    {
        [SerializeField] private TMP_InputField inputField;
        [SerializeField] private GameObject nameErrorText;

        public event Action PlayerNameSetEventTriggered;

        public void SetPlayerName()
        {
            string rawName = inputField.text.Trim();

            if (string.IsNullOrEmpty(rawName))
            {
                ShowNameError($"your name isn't '<i>empty</i>, is it?'");
                return;
            }

            string sanitizedName = SanitizeFileName(rawName);
            GlobalFileManager.Instance.SetPlayerName(sanitizedName);
            GlobalGameManager.Instance.FirstLogin = false;
            PlayerNameSetEventTriggered?.Invoke();
        }

        string SanitizeFileName(string name)
        {
            var invalidChars = Path.GetInvalidFileNameChars();
            return string.Join("_", name.Split(invalidChars));
        }

        void ShowNameError(string message)
        {
            nameErrorText.SetActive(true);
            nameErrorText.GetComponent<TMP_Text>().text = message;
        }
    }
}