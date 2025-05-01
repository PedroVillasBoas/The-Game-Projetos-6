namespace GoodVillageGames.Game.Enums.Pooling
{
    public enum PoolID
    {
        // Default for error handling if I forgot to set it up
        None,
        // Player
        PlayerProjectile,
        PlayerMissile,
        // Enemies
            // First MINION Type of each Difficulty
        FirstEasyMinionPrefab,
        FirstMediumMinionPrefab,
        FirstHardMinionPrefab,
            // Second MINION Type of each Difficulty
        SecondEasyMinionPrefab,
        SecondMediumMinionPrefab,
        SecondHardMinionPrefab,
            // First BOSS Type of each Difficulty
        FirstEasyBossPrefab,
        FirstMediumBossPrefab,
        FirstHardBossPrefab,
            // Second BOSS Type of each Difficulty
        SecondEasyBossPrefab,
        SecondMediumBossPrefab,
        SecondHardBossPrefab,
        // Enemy Projectiles
            // MINIONS
                // First
        FirstEasyMinionProjectile,
        FirstMediumMinionProjectile,
        FirstHardMinionProjectile,
                // Second
        SecondEasyMinionProjectile,
        SecondMediumMinionProjectile,
        SecondHardMinionProjectile,
            // BOSS
                // First
        FirstEasyBossProjectile,
        FirstMediumBossProjectile,
        FirstHardBossProjectile,
                // Second
        SecondEasyBossProjectile,
        SecondMediumBossProjectile,
        SecondHardBossProjectile,
                // SPECIAL PROJECTILE
                    // First
        FirstEasyBossSpecialProjectile,
        FirstMediumBossSpecialProjectile,
        FirstHardBossSpecialProjectile,
                    // Second
        SecondEasyBossSpecialProjectile,
        SecondMediumBossSpecialProjectile,
        SecondHardBossSpecialProjectile,
        // Pickups
            // EXP
        PickupEXPTiny,
        PickupEXPSmall,
        PickupEXPLarge,
        PickupEXPGigantic,
            // Items
        PickupItemDamage,
        PickupItemSpeed,
        PickupItemAttackSpeed,
        // Other
        DamageNumbers,
    }
}
