using System;
using UnityEngine;
using GoodVillageGames.Game.Enums.Stats;
using GoodVillageGames.Game.Core.GameObjectEntity;

namespace GoodVillageGames.Game.Core.Attributes.Modifiers
{
    public class Upgrade
    {
        [SerializeField] private StatType _statType;
        [SerializeField] private OperatorType _operatorType;
        [SerializeField] private float _value;
        private float _duration = -1f;

        public StatType StatType => _statType;
        public float Value => _value;

        public void ApplyUpgrade(Entity entity)
        {
            StatModifier modifier = _operatorType switch
            {
                OperatorType.Add => new BasicStatModifier(_statType, _duration, v => v + _value),
                OperatorType.Multiply => new BasicStatModifier(_statType, _duration, v => v * _value),
                OperatorType.Sub => new BasicStatModifier(_statType, _duration, v => v - _value),
                _ => throw new ArgumentOutOfRangeException()
            };

            entity.Stats.Mediator.AddModifier(modifier);
        }
    }
}