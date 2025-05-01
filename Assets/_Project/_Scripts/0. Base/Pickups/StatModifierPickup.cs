using System;
using UnityEngine;
using GoodVillageGames.Game.Enums.Stats;
using GoodVillageGames.Game.Core.Attributes;
using GoodVillageGames.Game.Core.GameObjectEntity;
using GoodVillageGames.Game.Core.Attributes.Modifiers;

namespace GoodVillageGames.Game.Core.Pickups
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