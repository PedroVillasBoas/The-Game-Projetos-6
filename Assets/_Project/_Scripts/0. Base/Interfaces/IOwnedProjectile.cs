using static GoodVillageGames.Game.Enums.Enums;

namespace GoodVillageGames.Game.Interfaces
{
    public interface IOwnedProjectile 
    {
        ProjectileType Type { get; }
        bool IsPlayerProjectile { get; }
    }
}