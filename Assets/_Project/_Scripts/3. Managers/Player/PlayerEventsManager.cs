using UnityEngine;
using UnityEngine.Events;
using GoodVillageGames.Game.Interfaces;

namespace GoodVillageGames.Game.Core.Manager.Player
{
    public class PlayerEventsManager : MonoBehaviour
    {
        // Events
            // Movement
        public UnityEvent<Vector2> OnPlayerMovingEventTriggered;
            // Boost
        public UnityEvent<bool> OnPlayerBoostingEventTriggered;
            // Projectiles
        public UnityEvent<bool> OnPlayerBulletEventTriggered;
        public UnityEvent<bool> OnPlayerMissileEventTriggered;
                // Cursor
        public UnityEvent OnPlayerProjectileEventTriggered;
            // VFX
        public UnityEvent<bool> OnPlayerBoostVFXEventTriggered;

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
        public void PlayerProjectileEvent()
        {
            OnPlayerProjectileEventTriggered?.Invoke();
        }
        public void PlayerBoostVFXEvent(bool value)
        {
            OnPlayerBoostVFXEventTriggered?.Invoke(value);
        }
    }
}
