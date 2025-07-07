using System;
using UnityEngine;
using GoodVillageGames.Game.Core.Util;
using GoodVillageGames.Game.Interfaces;
using GoodVillageGames.Game.Core.Attributes;
using GoodVillageGames.Game.Core.GameObjectEntity;
using MoreMountains.Feedbacks;

namespace GoodVillageGames.Game.Core.Enemy
{
    public class EnemyHealthHandler : MonoBehaviour, IDamageable, IVisitable
    {
        [SerializeField] private GameObject damagePrefab;
        [SerializeField] private MMF_Player feedbackFloatingText;

        private float _currentHealth;
        private Stats _stats;
        private IDTriggerOwner owner;
    
        public event Action OnDeath;
        public event Action<float> OnHealthChanged;
        public event Action<float> OnMaxHealthChanged;

        public float CurrentHealth 
        {
            get => _currentHealth;
            set => _currentHealth = Mathf.Clamp(value, 0, _stats.MaxHealth);
        }

        void Awake() => owner = GetComponentInParent<IDTriggerOwner>();

        void OnEnable()
        {
            _stats = transform.parent.parent.GetComponent<Entity>().Stats;
            _currentHealth = _stats.MaxHealth;
            OnHealthChanged?.Invoke(_currentHealth / _stats.MaxHealth);
        }

        public void TakeDamage(float amount)
        {
            _currentHealth = Mathf.Max(_currentHealth - amount, 0);

            OnHealthChanged?.Invoke(_currentHealth / _stats.MaxHealth);
            feedbackFloatingText.PlayFeedbacks(this.transform.position, amount);
            owner.NotifySubscribers("enemy-hit");
            Instantiate(damagePrefab, transform.position, Quaternion.identity);

            if (_currentHealth <= 0)
            {
                owner.NotifySubscribers("enemy-death");
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