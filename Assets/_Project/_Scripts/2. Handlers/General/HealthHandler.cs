using System;
using UnityEngine;
using UnityEngine.Events;
using GoodVillageGames.Game.Interfaces;
using GoodVillageGames.Game.Core.Attributes;
using GoodVillageGames.Game.Core.GameObjectEntity;

namespace GoodVillageGames.Game.Handlers
{
    public class HealthHandler : MonoBehaviour, IDamageable
    {
        private float _currentHealth;
        private Stats _entityStats;

        public event Action OnDeath;
        public event Action<float> OnHealthChanged;
        public event Action<float> OnMaxHealthChanged;

        private void Awake()
        {
            _entityStats = transform.root.GetComponent<Entity>().Stats;
            _currentHealth = _entityStats.MaxHealth;
        }

        private void Start()
        {
            OnHealthChanged?.Invoke(_currentHealth);
        }

        public void TakeDamage(float amount)
        {
            _currentHealth = Mathf.Max(_currentHealth - amount, 0);
            OnHealthChanged?.Invoke(_currentHealth);

            if (_currentHealth <= 0)
            {
                OnDeath?.Invoke();
                HandleDeath();
            }
        }

        private void HandleDeath()
        {
            // Handle death logic here
            gameObject.SetActive(false);
        }

        public Vector2 GetPosition() => transform.position;
    }
}