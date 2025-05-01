using GoodVillageGames.Game.Enums.Projectiles;

namespace GoodVillageGames.Game.Interfaces
{
    public interface IOwnedProjectile 
    {
        ProjectileType Type { get; }
        bool IsPlayerProjectile { get; }
    }
}