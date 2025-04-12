using UnityEngine;

namespace GoodVillageGames.Game.Core.Manager
{
    public class BackgroundStarsManager : MonoBehaviour
    {
        [SerializeField] private PlayerActions _playerActions;
        [SerializeField] private ParticleSystem _stars;

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
            var starVelocity = _stars.velocityOverLifetime;
            Vector2 playerVelocity = -GetPlayerLinearVelocity();

            starVelocity.x = new ParticleSystem.MinMaxCurve(playerVelocity.x);
            starVelocity.y = new ParticleSystem.MinMaxCurve(playerVelocity.y);
        }
    }
}
