using System;
using GoodVillageGames.Game.Core.Attributes;
using GoodVillageGames.Game.Core.Attributes.Modifiers;
using GoodVillageGames.Game.Core.GameObjectEntity;
using UnityEngine;
using static GoodVillageGames.Game.Enums.Enums;

namespace GoodVillageGames.Game.Core.Pickup
{
    public class StatModifierPickup : Pickup
    {
        [SerializeField] private StatType _statType;
        [SerializeField] private OperatorType _operatorType;
        [SerializeField] private float _value;
        [SerializeField] private float _duration;

        protected override void ApplyPickupEffect(Entity entity)
        {
            StatModifier modifier = _operatorType switch
            {
                OperatorType.None => throw new ArgumentException($"Pickup {gameObject} does not have a OperatorType!"),
                OperatorType.Add => new BasicStatModifier(_statType, _duration, v => v + _value),
                OperatorType.Multiply => new BasicStatModifier(_statType, _duration, v => v * _value),
                _ => throw new ArgumentOutOfRangeException()
            };

            entity.Stats.Mediator.AddModifier(modifier);
        }
    }
}