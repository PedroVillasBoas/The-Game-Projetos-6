using UnityEngine;
using TriInspector;
using GoodVillageGames.Game.Enums;
using GoodVillageGames.Game.Core.Global;

namespace GoodVillageGames.Game.Core.Manager
{
    public class CursorManager : MonoBehaviour
    {
        [Title("Game Cursor")]
        public GameObject gameCursorPrefab;
        private GameObject instanceGameCursor;

        public bool IsInGame { get; private set; }

        void OnEnable()
        {
            if (gameCursorPrefab != null)
            {
                instanceGameCursor = Instantiate(gameCursorPrefab, transform, false);
                instanceGameCursor.SetActive(false);
            }
            GlobalEventsManager.Instance.ChangeGameStateEventTriggered += ToggleCursor;
        }

        void OnDisable()
        {
            if (GlobalEventsManager.Instance != null)
            {
                GlobalEventsManager.Instance.ChangeGameStateEventTriggered -= ToggleCursor;
            }
        }

        void Update()
        {
            if (IsInGame && instanceGameCursor != null && instanceGameCursor.activeSelf)
            {
                instanceGameCursor.transform.position = Input.mousePosition;
            }
        }

        void ToggleCursor(GameState state)
        {
            IsInGame = state == GameState.GameBegin || state == GameState.GameContinue || state == GameState.Tutorial;

            Cursor.visible = !IsInGame;

            if (instanceGameCursor != null)
            {
                instanceGameCursor.SetActive(IsInGame);
            }
        }
    }
}