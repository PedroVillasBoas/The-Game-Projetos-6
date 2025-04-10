using static GoodVillageGames.Game.Enums.Enums;

namespace GoodVillageGames.Game.Core.Attributes
{
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