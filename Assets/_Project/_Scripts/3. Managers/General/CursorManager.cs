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

        public bool isInGame = true;

        void OnDisable()
        {
            GlobalEventsManager.Instance.ChangeGameStateEventTriggered -= ToggleCursor;
        }

        void Start()
        {
            GlobalEventsManager.Instance.ChangeGameStateEventTriggered += ToggleCursor;
        }

        void Update()
        {
            if (isInGame)
            {
                EnableAimCursor();
            }
            else
            {
                EnableUICursor();
            }
        }

        void EnableAimCursor()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.None;

            if (instanceGameCursor == null && gameCursorPrefab != null)
            {
                instanceGameCursor = Instantiate(gameCursorPrefab, transform, false);
            }

            if (instanceGameCursor != null)
            {
                Vector3 cursorPos = Input.mousePosition;
                instanceGameCursor.transform.position = cursorPos;
            }
        }

        void EnableUICursor()
        {
            Cursor.visible = true;

            if (instanceGameCursor != null)
            {
                Destroy(instanceGameCursor);
            }
        }

        void ToggleCursor(GameState state)
        {
            if (state == GameState.GameBegin || state == GameState.GameContinue)
                isInGame = true;
            else
                isInGame = false;
        }
    }
}
