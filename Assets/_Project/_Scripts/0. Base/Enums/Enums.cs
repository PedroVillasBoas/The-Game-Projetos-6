namespace GoodVillageGames.Game.Enums
{
    public class Enums
    {
        public enum AnimationID
        {
            FADE,
            MOVE_TO,
            SCTRETCH,
            PULSE,
            HOLOGRAM,
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
            MAIN_MENU,
            IN_GAME,
            UPGRADE_SCREEN,
            PAUSED,
            PLAYER_DIED,
            GAME_OVER,
        }

        public enum UIButtonActionType
        {
            UI_CHANGE,
            SCENE_CHANGE,
        }

        public enum UIAnimationType
        {
            SEQUENTIAL,
            PARALLEL,
        }

        public enum UIScreenID
        {
            NONE,
            MAIN_MENU,
            MM_DIFICULTY_SELECT,
            MM_OPTIONS,
            PLAYER_HUD,
            PAUSED,
            MODS,
            SETTINGS,
            GAME_OVER,
        }

        public enum ProjectileStatsEnum
        {
            PROJECTILE_DAMAGE,
            PROJECTILE_AMOUNT,
            PROJECTILE_TRAVEL_SPEED,
            PROJECTILE_LIFETIME,
            PROJECTILE_EXPLOSION_RADIUS,
        }

        public enum PoolID
        {
            NONE,
            PLAYER_PROJECTILE,
            PLAYER_MISSILE,
            ENEMY_PROJECTILE,
            ENEMY_PREFAB,
            ENEMY_MISSILE,
        }

        public enum StatType
        {
            None,
            MaxHealth,
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

        public enum UpgradeType
        {
            None,
            OnHit,
            OnTimer,
            OnStatModifier,
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
    }
}
