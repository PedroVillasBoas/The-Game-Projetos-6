namespace GoodVillageGames.Game.Enums
{
    public partial class Enums
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
    }
}
