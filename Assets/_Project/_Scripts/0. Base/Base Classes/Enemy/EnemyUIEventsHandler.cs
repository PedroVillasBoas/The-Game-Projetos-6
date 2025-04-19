using UnityEngine;
using UnityEngine.UI;

namespace GoodVillageGames.Game.Core.Enemy.UI
{
    public class EnemyUIEventsHandler : MonoBehaviour
    {
        [SerializeField] private EnemyHealthHandler enemyHealthHandler;
        [SerializeField] private Image enemyHpFill;

        void OnEnable()
        {
            UpdateEnemyHPFill(enemyHealthHandler.CurrentHealth);
        }

        void OnDisable()
        {
            enemyHealthHandler.OnHealthChanged -= UpdateEnemyHPFill; 
        }

        void Start()
        {
            enemyHealthHandler.OnHealthChanged += UpdateEnemyHPFill; 
        }

        void OnDestroy()
        {
            enemyHealthHandler.OnHealthChanged -= UpdateEnemyHPFill; 
        }

        void UpdateEnemyHPFill(float enemyHealthAmount)
        {
            if (enemyHpFill != null)
            {
                enemyHpFill.fillAmount = Mathf.Clamp01(enemyHealthAmount);
            }
        }
    }
}
