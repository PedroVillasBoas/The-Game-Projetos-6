using System;
using static GoodVillageGames.Game.Enums.Enums;

namespace GoodVillageGames.Game.Core.Attributes.Modifiers
{
    /// <summary>
    /// A Simple Stat Modifier/Upgrade that will do a operation to the Stat
    /// </summary>
    public class BasicStatModifier : StatModifier
    {
        readonly StatType type;
        readonly Func<float, float> operation;

        public BasicStatModifier(StatType type, float duration, Func<float, float> operation) : base(duration)
        {
            this.type = type;
            this.operation = operation;
        }

        public override void Handle(object sender, Query query)
        {
            if (query.StatType == type)
            {
                query.Value = operation(query.Value);
            }
        }
    }
}