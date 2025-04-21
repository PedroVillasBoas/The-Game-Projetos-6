using UnityEngine;
using UnityEngine.InputSystem;
using GoodVillageGames.Game.Handlers;
using GoodVillageGames.Game.Core.Global;
using static GoodVillageGames.Game.Enums.Enums;

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
            _playerInputComponent.onActionTriggered += OnActionTriggered;
        }

        void OnDisable()
        {
            _playerInputComponent.onActionTriggered -= OnActionTriggered;
        }

        void OnDestroy()
        {
            _inputActions.Player.Disable();
        }

        void Update()
        {
            OnUIPlayingAnimation();

            if (CheckGameStates())
            {
                Vector2 currentMousePos = Mouse.current.position.ReadValue();
                _playerAimHandler.HandleLook(currentMousePos);
            }

        }

        void OnActionTriggered(InputAction.CallbackContext context)
        {
            if (CheckGameStates())
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

        bool CheckGameStates()
        {
            return (GlobalGameManager.Instance.GameState == GameState.GameBegin || GlobalGameManager.Instance.GameState == GameState.GameContinue) && GlobalGameManager.Instance.UIState == UIState.NoAnimationPlaying;
        }

        void OnUIPlayingAnimation()
        {
            switch (GlobalGameManager.Instance.UIState)
            {
                case UIState.NORMAL_UI:
                    if (_inputActions.Player.enabled)
                        _inputActions.Player.Disable();
                    break;

                case UIState.PLAYING_UI_ANIM:
                    if (_inputActions.Player.enabled)
                        _inputActions.Player.Disable();
                    break;

                case UIState.NoAnimationPlaying:
                    if (!_inputActions.Player.enabled)
                        _inputActions.Player.Enable();
                    break;
            }
        }

    }
}
