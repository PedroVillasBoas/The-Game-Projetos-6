using UnityEngine;
using UnityEngine.InputSystem;
using GoodVillageGames.Game.Handlers;
using static GoodVillageGames.Game.Enums.Enums;
using GoodVillageGames.Game.Core.Global;

namespace GoodVillageGames.Game.Core.Manager
{
    public class PlayerInputManager : MonoBehaviour
    {
        private InputSystem_Actions _inputActions;
        private PlayerInput _playerInputComponent;

        private PlayerActions _playerActions;
        private PlayerAimHandler _playerAimHandler;

        void Awake()
        {
            _inputActions = new InputSystem_Actions();
            _playerInputComponent = GetComponent<PlayerInput>();
            _playerActions = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerActions>();
            _playerAimHandler = _playerActions.gameObject.GetComponentInChildren<PlayerAimHandler>();
        }

        void OnEnable()
        {
            EventsManager.Instance.OnAnimationEventTriggered += OnUIPlayingAnimation;
            _playerInputComponent.onActionTriggered += OnActionTriggered;
            GlobalEventsManager.Instance.ChangeGameStateEventTriggered += ToggleControls;
        }

        void OnDisable()
        {
            _playerInputComponent.onActionTriggered -= OnActionTriggered;
            EventsManager.Instance.OnAnimationEventTriggered -= OnUIPlayingAnimation;
            GlobalEventsManager.Instance.ChangeGameStateEventTriggered -= ToggleControls;
        }

        void Update()
        {
            if (GlobalGameManager.Instance.GameState == GameState.GameBegin ||
            GlobalGameManager.Instance.GameState == GameState.GameContinue)
            {
                Vector2 currentMousePos = Mouse.current.position.ReadValue();
                _playerAimHandler.HandleLook(currentMousePos);
            }
        }

        void OnActionTriggered(InputAction.CallbackContext context)
        {
            switch (context.action.name)
            {
                case "Move":
                    OnMove(context);
                    break;

                case "Attack":
                    OnAttack(context);
                    break;

                case "Boost":
                    OnBoost(context);
                    break;

                case "Missile":
                    OnMissile(context);
                    break;
            }
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            Vector2 _playerPosition = context.ReadValue<Vector2>();
            _playerActions.HandleMove(_playerPosition);
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            if (context.performed)
                _playerActions.HandleAttack(true);
            else if (context.canceled)
                _playerActions.HandleAttack(false);
        }

        public void OnBoost(InputAction.CallbackContext context)
        {
            if (context.performed)
                _playerActions.HandleBoost(true);
            else if (context.canceled)
                _playerActions.HandleBoost(false);
        }

        public void OnMissile(InputAction.CallbackContext context)
        {
            if (context.performed)
                _playerActions.HandleMissile(true);
            else if (context.canceled)
                _playerActions.HandleMissile(false);
        }

        public void ToggleControls(GameState gameState)
        {
            if(gameState == GameState.MainMenu || gameState == GameState.GamePaused || gameState == GameState.PlayerDied || gameState == GameState.GameOver)
            {
                //_inputActions.Player.Disable();
                _playerInputComponent.onActionTriggered -= OnActionTriggered;
                _inputActions.UI.Enable();
            }
            else
            {
                //_inputActions.Player.Enable();
                _inputActions.UI.Disable();
                _playerInputComponent.onActionTriggered += OnActionTriggered;
            }
        }

        void OnUIPlayingAnimation(UIState _UIState)
        {
            switch (_UIState)
            {
                case UIState.NORMAL_UI:
                    _inputActions.UI.Enable();
                    break;
                
                case UIState.PLAYING_UI_ANIM:
                    _inputActions.UI.Disable();
                    break;
                
                default:
                    Debug.Log($"No UIState Case for: '{_UIState}'!");
                    break;
            }
        }

    }
}
