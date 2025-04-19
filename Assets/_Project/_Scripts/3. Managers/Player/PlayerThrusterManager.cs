using UnityEngine;
using System.Collections.Generic;
using GoodVillageGames.Game.Core.Util;
using GoodVillageGames.Game.Core.Manager.Player;

namespace GoodVillageGames.Game.Core.Manager
{
    public class PlayerThrusterManager : MonoBehaviour
    {
        private List<ParticleSystem> _thrusters = new();
        private PlayerEventsManager _playerEventsManager;

        void Awake()
        {
            foreach (Transform child in transform)
            {
                if (child.TryGetComponent<ParticleSystem>(out var ps))
                    _thrusters.Add(ps);
            }
        }

        void Start()
        {
            if (transform.root.TryGetComponent<PlayerActions>(out var player))
            {
                _playerEventsManager = player.PlayerEventsManager;
            }

            _playerEventsManager.OnPlayerMovingEventTriggered.AddListener(OnPlayerMovingEvent);
            _playerEventsManager.OnPlayerBoostVFXEventTriggered.AddListener(OnPlayerBoostingEvent);
        }

        void OnPlayerMovingEvent(Vector2 vector)
        {
            if (Utillities.Vector2DifferentThanZero(vector))
                ActivateThrusters();
            else
                DeactivateThrusters();
        }

        void OnPlayerBoostingEvent(bool value)
        {
            if (value)
                SetBoosting();
            else
                UnsetBoosting();
        }

        void ActivateThrusters()
        {
            foreach (ParticleSystem child in _thrusters)
            {
                if (!child.isEmitting)
                    child.Play();
            }
        }

        void DeactivateThrusters()
        {
            foreach (ParticleSystem child in _thrusters)
            {
                if (child.isEmitting)
                    child.Stop();
            }
        }

        void SetBoosting()
        {
            foreach (ParticleSystem child in _thrusters)
            {
                var psRenderer = child.GetComponent<ParticleSystemRenderer>();
                psRenderer.lengthScale = 12;
            }
        }

        void UnsetBoosting()
        {
            foreach (ParticleSystem child in _thrusters)
            {
                var psRenderer = child.GetComponent<ParticleSystemRenderer>();
                psRenderer.lengthScale = 3;
            }
        }


    }
}
