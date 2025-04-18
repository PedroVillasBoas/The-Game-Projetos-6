namespace GoodVillageGames.Game.Enums
{
    public class Enums
    {
        public enum AnimationID
        {
            Fade,
            MoveTo,
            Scretch,
            Pulse,
            Hologram,
        }

        public enum AnimationTransitionID
        {
            // Basic
            NONE,
            SPLASH_SCREEN_TO_MAIN_MENU,
            // Main Menu
            DEFAULT_MAIN_MENU,
            MAIN_MENU_TO_DIFFICULTY_SELECT,
            DIFFICULTY_SELECT_TO_PLAY_GAME,
            RETURN_DIFFICULTY_SELECT_TO_MAIN_MENU,
            MAIN_MENU_TO_OPTIONS,
            RETURN_OPTIONS_SELECT_TO_MAIN_MENU,
            // In Game
            DEFAULT_PLAYER_GUI,
            PLAYER_GUI_TO_PAUSE,
            DEFAULT_PAUSE,
            RETURN_PAUSE_TO_PLAYER_GUI,
            DEFAULT_IN_GAME_QUIT_POPUP,
            // Popup
            OPEN_POPUP,
            CLOSE_POPUP,
            // Options Specific
            DEFAULT_OPTIONS,
            OPTIONS_MODS_TO_SETTINGS,
            OPTIONS_SETTINGS_TO_MODS,
            // Upgrade - Mods
            DEFAULT_UPGRADES,
            CLOSE_UPGRADES,
            // Game Over
            DEFAULT_GAME_OVER,
            GAME_OVER_TO_MAIN_MENU,
            // Difficulty Select
            DEFAULT_DIFFICULTY_SELECT,
        }

        public enum UIState
        {
            PLAYING_UI_ANIM,
            NORMAL_UI,
        }

        public enum GameState
        {
            MainMenu,
            InGame,
            UpgradeScreen,
            Paused,
            PlayerDied,
            GameOver,
        }

        public enum UIButtonActionType
        {
            UIChange,
            SceneChange,
        }

        public enum UIAnimationType
        {
            Sequential,
            Paarallel,
        }

        public enum UIScreenID
        {
            None,
            MainMenu,
            MMtoDifficultySelect,
            MMtoOptions,
            PlayerHUD,
            Paused,
            Mods,
            Settings,
            GameOver,
        }

        public enum ProjectileStatsEnum
        {
            ProjectileDamage,
            ProjectileAmount,
            ProjectileTravelSpeed,
            ProjectileLifetime,
            ProjectileExplosionRadius,
        }

        public enum PoolID
        {
            None,
            PlayerProjectile,
            PlayerMissile,
            EnemyMinionPrefab,
            EnemyBossPrefab,
            EnemyMinionProjectile,
            EnemyBossProjectile,
            PickupEXPTiny,
            PickupEXPSmall,
            PickupEXPLarge,
            PickupEXPGigantic,
            PickupItemDamage,
            PickupItemSpeed,
            PickupItemAttackSpeed,
            EnemyMinionSecondPrefab,
            EnemyBossSecondPrefab,
            EnemyBossSpecialProjectile,
        }

        public enum StatType
        {
            None,
            MaxHealth,
            CurrentHealth,
            MaxSpeed,
            MaxDefense,
            BaseAttackDamage,
            AttackSpeed,
            MaxBoostTime,
            MaxBoostSpeed,
            BaseMissileDamage,
            BaseMissileCooldown,
            BoostRechargeRate,
            Acceleration,
            Experience,
        }

        public enum OperatorType
        {
            None,
            Add,
            Multiply,
        }
        public enum UpgradeRarity
        {
            None,
            Common,
            Uncommon,
            Rare,
            Epic,
            Legendary,
        }

        public enum EnemyType
        {
            MinionEasyFirst,
            MinionEasySecond,
            MinionMediumFirst,
            MinionMediumSecond,
            MinionHardFirst,
            MinionHardSecond,
            BossEasyFirst,
            BossEasySecond,
            BossMediumFirst,
            BossMediumSecond,
            BossHardFirst,
            BossHardSecond,
        }
    }
}
