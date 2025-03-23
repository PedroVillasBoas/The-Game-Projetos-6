namespace GoodVillageGames.Game.Enums
{
    public class Enums
    {
        public enum AnimationID {
            SPLASH_SCREEN,
            MAIN_MENU,
            MM_DIFFICULTY,
            MM_DIFFICULTY_MM,
            MM_OPTIONS,
            MM_OPTIONS_MM,
            IG_PAUSE,
            IG_PAUSE_IG,
            MODS,
            OPTIONS,
            QUIT,
            QUIT_POPUP,
            MM_IG,
            IG_MM,
        }

        public enum UIState {
            PLAYING_UI_ANIM,
            NORMAL_UI,
        }

        public enum GameState {
            MAIN_MENU,
            IN_GAME,
            UPGRADE_SCREEN,
            PAUSED,
            PLAYER_DIED,
            GAME_OVER,
        }

        public enum UIButtonActionType {
            UI_CHANGE,
            SCENE_CHANGE,
        }

        public enum UIAnimationType {
            SEQUENTIAL,
            PARALLEL,
        }
    }
}
