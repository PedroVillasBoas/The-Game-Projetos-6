using UnityEngine;
using GoodVillageGames.Game.Core;
using GoodVillageGames.Game.Enums;
using GoodVillageGames.Game.Core.Global;

namespace GoodVillageGames.Game.Handlers
{
    public class PlayerDeathHandler : MonoBehaviour 
    { 
        [SerializeField] private GameObject deathVFX;
        [SerializeField] private GameObject playerVisuals;

        void Start() => GlobalEventsManager.Instance.ChangeGameStateEventTriggered += OnPlayerDied;
        void OnDisable() => GlobalEventsManager.Instance.ChangeGameStateEventTriggered -= OnPlayerDied;

        void OnPlayerDied(GameState gameState)
        {
            if (gameState == GameState.PlayerDied)
            {
                PlayerAudioHandler.Instance.PlayPlayerDeathSFX();
                playerVisuals.SetActive(false);
                deathVFX.SetActive(true);
            }
        }
    }
}