using System.Collections;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace GoodVillageGames.Game.Core.Manager.Localization
{
    public class LocalizationManager : MonoBehaviour
    {
        private void Start()
        {
            int id = PlayerPrefs.GetInt("LocaleKey", 0);
            StartCoroutine(SetLocale(id));
        }

        public void ChangeLocale(int localeID)
        {
            StartCoroutine(SetLocale(localeID));
        }

        IEnumerator SetLocale(int localeID)
        {
            yield return LocalizationSettings.InitializationOperation;

            if (localeID >= 0 && localeID < LocalizationSettings.AvailableLocales.Locales.Count)
            {
                LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[localeID];
                PlayerPrefs.SetInt("LocaleKey", localeID);
            }
            else
            {
                Debug.LogWarning("Locale ID inválido: " + localeID);
            }
        }
    }
}