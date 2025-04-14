using System;
using UnityEngine;

namespace GoodVillageGames.Game.Core.Enemy.AI
{
    public class EnemyDetectionTrigger : MonoBehaviour 
    {
        public Action<bool> PlayerInRangeActionTriggered;

        [HideInInspector] public CircleCollider2D Collider;

        void Awake()
        {
            Collider = GetComponent<CircleCollider2D>();
        }

        // Player Entered Area Detection to Fire
        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                PlayerInRangeActionTriggered?.Invoke(true);
            }
        }

        // Player is on Fire Area
        void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                PlayerInRangeActionTriggered?.Invoke(true);
            }
        }

        // Player Left Fire Area
        void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                PlayerInRangeActionTriggered?.Invoke(false);
            }
        }
    }
}
