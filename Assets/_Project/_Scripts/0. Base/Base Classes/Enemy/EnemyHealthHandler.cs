using System;
using UnityEngine;
using GoodVillageGames.Game.Interfaces;
using GoodVillageGames.Game.Core.Attributes;
using GoodVillageGames.Game.Core.GameObjectEntity;

namespace GoodVillageGames.Game.Core.Enemy
{
    public class EnemyHealthHandler : MonoBehaviour, IDamageable, IVisitable
    {
        private float _currentHealth;
        private Stats _stats;
    
        public event Action OnDeath;
        public event Action<float> OnHealthChanged;
        public event Action<float> OnMaxHealthChanged;

        public float CurrentHealth 
        {
            get => _currentHealth;
            set => _currentHealth = Mathf.Clamp(value, 0, _stats.MaxHealth);
        }

        private void Start()
        {
            _stats = transform.root.GetComponent<Entity>().Stats;
            _currentHealth = _stats.MaxHealth;
            OnHealthChanged?.Invoke(_currentHealth / _stats.MaxHealth);
        }

        public void TakeDamage(float amount)
        {
            _currentHealth = Mathf.Max(_currentHealth - amount, 0);

            OnHealthChanged?.Invoke(_currentHealth / _stats.MaxHealth);

            if (_currentHealth <= 0)
            {
                OnDeath?.Invoke();
            }
        }

        public Vector2 GetPosition() => transform.position;

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        } 
    }
}