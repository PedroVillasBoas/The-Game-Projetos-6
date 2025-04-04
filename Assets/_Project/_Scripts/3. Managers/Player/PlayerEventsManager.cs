using UnityEngine;
using UnityEngine.Events;

namespace GoodVillageGames.Game.Core.Manager.Player
{
    public class PlayerEventsManager : MonoBehaviour
    {
        // Events
            // Movement
        public UnityEvent<Vector2> OnPlayerMovingEventTriggered;
            // VFX
        public UnityEvent<bool> OnPlayerBoostingEventTriggered;
            // Projectiles
        public UnityEvent<bool> OnPlayerBulletEventTriggered;
        public UnityEvent<bool> OnPlayerMissileEventTriggered;


        public void PlayerMovingEvent(Vector2 value)
        {
            OnPlayerMovingEventTriggered?.Invoke(value);
        }

        public void PlayerBoostingEvent(bool value)
        {
            OnPlayerBoostingEventTriggered?.Invoke(value);
        }

        public void PlayerBulletEvent(bool value)
        {
            OnPlayerBulletEventTriggered?.Invoke(value);
        }

        public void PlayerMissileEvent(bool value)
        {
            OnPlayerMissileEventTriggered?.Invoke(value);
        }
    }
}
