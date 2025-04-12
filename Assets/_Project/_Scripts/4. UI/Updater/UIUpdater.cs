using UnityEngine;
using UnityEngine.UI;
using GoodVillageGames.Game.Core.Manager.UI;
using System.Collections;

namespace GoodVillageGames.Game.General.UI.Updater
{
    public class UIUpdater : MonoBehaviour
    {
        [SerializeField] private Image _fuelFill;

        [SerializeField] private Image _missileFill;
        private Coroutine _missileCoroutine;

        void Start()
        {
            if (UIEventsManager.Instance != null)
            {
                UIEventsManager.Instance.PlayerUIBoostEventTriggered += UpdateFuelUI;
                UIEventsManager.Instance.PlayerUIMissileEventTriggered += UpdateMissileUI;
                // UIEventsManager.Instance.PlayerUIHealthEventTriggered += UpdateFuelUI;
            }
            else
                Debug.LogError("UIEventsManager instance not found!");
        }

        void OnDestroy()
        {
            if (UIEventsManager.Instance != null)
                UIEventsManager.Instance.PlayerUIBoostEventTriggered -= UpdateFuelUI;
        }

        void UpdateFuelUI(float fuelAmount)
        {
            if (_fuelFill != null)
                _fuelFill.fillAmount = Mathf.Clamp01(fuelAmount);
            else
                Debug.LogWarning("Fuel Fill Image reference is missing!");
        }

        void UpdateMissileUI(float missileCooldownProgress)
        {
            if (_missileFill != null)
            {
                if (_missileCoroutine != null)
                    StopCoroutine(_missileCoroutine);
                
                _missileCoroutine = StartCoroutine(AnimateFill(_missileFill, missileCooldownProgress));
            }
            else
            {
                Debug.LogWarning("Missile Fill Image reference is missing!");
            }
        }

        IEnumerator AnimateFill(Image image, float duration)
        {
            float elapsedTime = 0f;
            image.fillAmount = 0f;
            
            while (elapsedTime < duration)
            {
                image.fillAmount = Mathf.Lerp(0f, 1f, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            
            // Just to make sure...
            image.fillAmount = 1f;
        }
    }
}