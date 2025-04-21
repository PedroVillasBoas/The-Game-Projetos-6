using UnityEngine;
using GoodVillageGames.Game.Core.Global;
using static GoodVillageGames.Game.Enums.Enums;

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
                playerVisuals.SetActive(false);
                deathVFX.SetActive(true);
            }
        }
    }
}