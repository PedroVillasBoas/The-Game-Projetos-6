using System;
using UnityEngine;


namespace GoodVillageGames.Game.Interfaces
{
    public interface IDamageable
    {
        Vector2 GetPosition();
        event Action OnDeath;
        event Action<float> OnHealthChanged;
        event Action<float> OnMaxHealthChanged;
        void TakeDamage(float amount);
    }
}
