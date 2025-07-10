using System.Collections;
using GoodVillageGames.Game.Core.Global;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace GoodVillageGames.Game.Core.Manager.Localization
{
    public class LocalizationManager : MonoBehaviour
    {
        private bool active = false;

        void Start()
        {
            int id = GlobalGameManager.Instance.LocaleID;
            ChangeLocale(id);
        }

        public void ChangeLocale(int localeID)
        {
            if (active) return;

            StartCoroutine(SetLocale(localeID));
        }

        IEnumerator SetLocale(int _localeID)
        {
            active = true;
            yield return LocalizationSettings.InitializationOperation;
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[_localeID];
            GlobalEventsManager.Instance.ChangeLocale(_localeID);
            active = false;
        }
    }
}