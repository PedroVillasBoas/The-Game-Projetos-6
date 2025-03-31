using UnityEngine;
using GoodVillageGames.Game.Core.Manager;


namespace GoodVillageGames.Game.General.UI.Animations
{
    public class MovingStars : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _starsParticleSystem;

        void OnToggleMovingStars(string value)
        {
            var temp = _starsParticleSystem.velocityOverLifetime;

            switch (value)
            {
                case "Move":
                        temp.radial = 30;
                        _starsParticleSystem.Play();
                        break;
                
                case "MoveBack":
                        temp.radial = -30;
                        _starsParticleSystem.Play();
                        break;
                
                case "Stop":
                    if (_starsParticleSystem.isPlaying)
                        _starsParticleSystem.Stop();
                        break;
                
                default:
                    Debug.Log("Nothing to stop on the Stars, it is not playing.");
                    break;

            }
        }

        void OnEnable()
        {
            EventsManager.Instance.OnEventTriggered += OnToggleMovingStars;
        }

        void OnDisable()
        {
            EventsManager.Instance.OnEventTriggered -= OnToggleMovingStars;
        }
    }
}
