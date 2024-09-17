using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GOBA
{
    public class InputSystemHeroController : IHeroInput
    {
        private Hero _hero;
        private readonly HeroInput _inputActions;
        private LayerMask _terrainMask;
        private PlayerCamera _playerCamera;

        public InputSystemHeroController(Hero hero, PlayerCamera playerCamera)
        {
            _inputActions = new HeroInput();
            _terrainMask = CONSTANTS.Layers.Terrain;
            _hero = hero;
            _playerCamera = playerCamera;
        }

        public Vector2 MousePosition => Mouse.current.position.ReadValue();

        public void Enable()
        {
            _inputActions.Enable();
            
            _inputActions.AbilitiesHotKeys.Use1.performed += (ctx) => AbilityUse(ctx, 0);
            _inputActions.AbilitiesHotKeys.Use2.performed += (ctx) => AbilityUse(ctx, 1);
            _inputActions.AbilitiesHotKeys.Use3.performed += (ctx) => AbilityUse(ctx, 2);
            _inputActions.Common.StopAction.performed += StopAction;
            _inputActions.Common.RightClick.performed += TryMove;
        }

        public void Disable()
        {
            _inputActions.Disable();

            _inputActions.AbilitiesHotKeys.Use1.performed -= (ctx) => AbilityUse(ctx, 0);
            _inputActions.AbilitiesHotKeys.Use2.performed -= (ctx) => AbilityUse(ctx, 1);
            _inputActions.AbilitiesHotKeys.Use3.performed -= (ctx) => AbilityUse(ctx, 2);
            _inputActions.Common.StopAction.performed -= StopAction;
            _inputActions.Common.RightClick.performed -= TryMove;
        }

        private void StopAction(InputAction.CallbackContext context)
        {
            _hero.CancelActionRpc();
        }

        private void AbilityUse(InputAction.CallbackContext context, int abilityIndex)
        {
            _hero.OnAbilityRequestedRpc(abilityIndex);
        }

        private void TryMove(InputAction.CallbackContext context)
        {
            if (Input.GetMouseButtonDown(1) && Utils.IsMouseOverUI() == false && GetMovePoint(out Vector3 movePoint))
            {
                _hero.Move(movePoint);
            }
        }

        private bool GetMovePoint(out Vector3 movePoint)
        {
            movePoint = Vector3.zero;
            var mouseScreenPosition = Input.mousePosition;
            var ray = _playerCamera.Camera.ScreenPointToRay(mouseScreenPosition);

            if (Physics.Raycast(ray, out RaycastHit hit, 200f, _terrainMask) == false)
                return false;

            movePoint = hit.point;
            return true;
        }

        public void SetAbilityCallback(Action<int> callback)
        {
            //throw new NotImplementedException();
        }

        public void SetStopActionCallback(Action callback)
        {
            //throw new NotImplementedException();
        }
    }
}