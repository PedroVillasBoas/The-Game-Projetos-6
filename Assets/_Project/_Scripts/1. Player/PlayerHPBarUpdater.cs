using DG.Tweening;
using GoodVillageGames.Game.Handlers;
using UnityEngine;
using UnityEngine.UI;

namespace GoodVillageGames.Game.General.UI
{
    public class PlayerHPBarUpdater : MonoBehaviour
    {
        [SerializeField] private HealthHandler _playerHealthHandler;

        private Image _hpBarFillImage;

        private void Awake()
        {
            _hpBarFillImage = GetComponent<Image>();
        }

        void OnEnable()
        {
            _playerHealthHandler.OnHealthChanged += UpdateBarFill;
        }

        void OnDisable()
        {
            _playerHealthHandler.OnHealthChanged -= UpdateBarFill;
        }


        private void UpdateBarFill(float amount)
        {
            amount = Mathf.Clamp01(amount);
            _hpBarFillImage.DOFillAmount(amount, 0.5f).SetEase(Ease.OutCubic);
        }

    }
}
