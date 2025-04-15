using UnityEngine;
using UnityEngine.UI;
using GoodVillageGames.Game.Core.Manager.UI;
using System.Collections;

namespace GoodVillageGames.Game.General.UI.Updater
{
    public class UIUpdater : MonoBehaviour
    {
        // Boost
        [SerializeField] private Image _fuelFill;

        // Missile
        [SerializeField] private Image _missileFill;
        private Coroutine _missileCoroutine;

        // Health
        [SerializeField] private Image _healthFill;
        [SerializeField] private ParticleSystem _damageParticle;
        [SerializeField] private ParticleSystem _healParticle;

        // EXP
        [SerializeField] private Image _expFill;

        void Start()
        {
            if (UIEventsManager.Instance != null)
            {
                UIEventsManager.Instance.PlayerUIBoostEventTriggered += UpdateFuelUI;
                UIEventsManager.Instance.PlayerUIMissileEventTriggered += UpdateMissileUI;
                UIEventsManager.Instance.PlayerUIHealthEventTriggered += UpdateHealthUI;
                UIEventsManager.Instance.PlayerUIExpEventTriggered += UpdateExpUI;
            }
            else
                Debug.LogError("UIEventsManager instance not found!");
        }

        void OnDestroy()
        {
            if (UIEventsManager.Instance != null)
            {
                UIEventsManager.Instance.PlayerUIBoostEventTriggered -= UpdateFuelUI;
                UIEventsManager.Instance.PlayerUIMissileEventTriggered -= UpdateMissileUI;
                UIEventsManager.Instance.PlayerUIHealthEventTriggered -= UpdateHealthUI;
                UIEventsManager.Instance.PlayerUIExpEventTriggered += UpdateExpUI;
            }
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

        void UpdateHealthUI(float healthAmount)
        {
            if (_healthFill != null)
            {
                float previousHealth = _healthFill.fillAmount;
                _healthFill.fillAmount = Mathf.Clamp01(healthAmount);
                StartCoroutine(PunchHealthUI());

                if (previousHealth > _healthFill.fillAmount)
                    _damageParticle.Play();
                else
                    _healParticle.Play();
            }
            else
            {
                Debug.LogWarning("Health Fill Image reference is missing!");
            }
        }

        IEnumerator PunchHealthUI()
        {
            // Original hp bar scale
            Vector3 originalScale = _healthFill.transform.localScale;
            float duration = 0.15f;
            float halfDuration = duration * 0.5f;
            float timer = 0f;

            // Scale the size
            while (timer < halfDuration)
            {
                float progress = timer / halfDuration;
                _healthFill.transform.localScale = Vector3.Lerp(originalScale, originalScale * 1.1f, progress);
                timer += Time.deltaTime;
                yield return null;
            }

            // Return to the original scale
            timer = 0f;
            while (timer < halfDuration)
            {
                float progress = timer / halfDuration;
                _healthFill.transform.localScale = Vector3.Lerp(originalScale * 1.1f, originalScale, progress);
                timer += Time.deltaTime;
                yield return null;
            }

            // Just making sure...
            _healthFill.transform.localScale = originalScale;
        }
    
        void UpdateExpUI(float expAmount)
        {
            if (_expFill != null)
                _expFill.fillAmount = Mathf.Clamp01(expAmount);
        }
    }
}