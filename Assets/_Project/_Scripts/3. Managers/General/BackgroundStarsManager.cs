using UnityEngine;

namespace GoodVillageGames.Game.Core.Manager
{
    public class BackgroundStarsManager : MonoBehaviour
    {
        [SerializeField] private PlayerActions _playerActions;
        [SerializeField] private ParticleSystem stars;

        void Update()
        {
            UpdateStarsVelocityOverTime();
        }

        Vector2 GetPlayerLinearVelocity()
        {
            return _playerActions.PlayerLinearVelocity;
        }

        void UpdateStarsVelocityOverTime()
        {
            var starVelocity = stars.velocityOverLifetime;
            Vector2 playerVelocity = GetPlayerLinearVelocity();

            starVelocity.x = new ParticleSystem.MinMaxCurve(-1 * playerVelocity.x);
            starVelocity.y = new ParticleSystem.MinMaxCurve(-1 * playerVelocity.y);
        }
    }
}
