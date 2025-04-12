using GoodVillageGames.Game.Core;
using GoodVillageGames.Game.Core.Manager.Player;
using UnityEngine;

namespace GoodVillageGames.Game.General.UI.Cursor
{
    public class CursorHandler : MonoBehaviour
    {
        private Animator _cursorAnimator;
        private PlayerEventsManager _playerEventsManager;
        private PlayerActions _playerActions;

        void Start()
        {
            _cursorAnimator = GetComponent<Animator>();

            _playerActions = FindFirstObjectByType<PlayerActions>();
            _playerEventsManager = _playerActions.PlayerEventsManager;

            _playerEventsManager.OnPlayerProjectileEventTriggered.AddListener(TriggerShootingAnimation);
            _playerEventsManager.OnPlayerProjectileEventTriggered.AddListener(TriggerShootingAnimation);
        }

        void OnDestroy()
        {
            _playerEventsManager.OnPlayerProjectileEventTriggered.RemoveListener(TriggerShootingAnimation);
            _playerEventsManager.OnPlayerProjectileEventTriggered.RemoveListener(TriggerShootingAnimation);

        }

        public void TriggerShootingAnimation()
        {
            _cursorAnimator.SetTrigger("PlayerShoot");
        }
    }
}
