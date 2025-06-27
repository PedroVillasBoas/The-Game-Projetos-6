using UnityEngine;

namespace GoodVillageGames.Game.Core.Manager
{
    public class GameBoundsManager : MonoBehaviour 
    {
        [SerializeField] private GameObject warningPopup;

        private BoxCollider2D bounds;
        


        void Awake()
        {
            bounds = GetComponent<BoxCollider2D>();
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent(out PlayerActions player))
            {
                if (warningPopup != null)
                    warningPopup.SetActive(false);
            }
        }

        void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent(out PlayerActions player))
            {
                if (warningPopup != null)
                    warningPopup.SetActive(true);
            }
        }
    }
}
