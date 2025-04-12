using UnityEngine;
using TriInspector;

namespace GoodVillageGames.Game.Core.Manager
{
    public class CursorManager : MonoBehaviour
    {
        [Title("Game Cursor (Prefab Animado)")]
        public GameObject gameCursorPrefab;
        private GameObject instanceGameCursor;

        // Change this later!!!
        public bool isInGame = true;

        void Update()
        {
            // Change this later!!
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
    }
}
