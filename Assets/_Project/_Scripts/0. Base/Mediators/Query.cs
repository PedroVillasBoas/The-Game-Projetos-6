using GoodVillageGames.Game.Enums.Stats;

namespace GoodVillageGames.Game.Core.Attributes
{
    /// <summary>
    /// Consult to all of the <param name="StatType"> with the <param name="Value"> in the chain;
    /// <see cref="StatsMediator"/>
    /// </summary>
    public class Query
    {
        public readonly StatType StatType;
        public float Value;

        public Query(StatType statType, float value)
        {
            StatType = statType;
            Value = value;
        }
    }
}