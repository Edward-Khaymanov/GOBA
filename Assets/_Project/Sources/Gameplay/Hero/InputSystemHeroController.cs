using System;
using UnityEngine.InputSystem;

namespace GOBA
{
    public class InputSystemHeroController : IHeroInput
    {
        private readonly HeroInput _inputActions;

        public InputSystemHeroController()
        {
            _inputActions = new HeroInput();
        }

        public event Action<int> AbilityRequested;
        public event Action StopActionRequsted;

        public void Enable()
        {
            _inputActions.AbilitiesHotKeys.Use1.performed += (ctx) => AbilityUse(ctx, 0);
            _inputActions.AbilitiesHotKeys.Use2.performed += (ctx) => AbilityUse(ctx, 1);
            _inputActions.AbilitiesHotKeys.Use3.performed += (ctx) => AbilityUse(ctx, 2);
            _inputActions.Common.StopAction.performed += StopAction;

            _inputActions.Enable();
        }

        public void Disable()
        {
            _inputActions.Disable();

            _inputActions.AbilitiesHotKeys.Use1.performed -= (ctx) => AbilityUse(ctx, 0);
            _inputActions.AbilitiesHotKeys.Use2.performed -= (ctx) => AbilityUse(ctx, 1);
            _inputActions.AbilitiesHotKeys.Use3.performed -= (ctx) => AbilityUse(ctx, 2);
            _inputActions.Common.StopAction.performed -= StopAction;
        }

        private void StopAction(InputAction.CallbackContext context)
        {
            StopActionRequsted?.Invoke();
        }

        private void AbilityUse(InputAction.CallbackContext context, int abilityIndex)
        {
            AbilityRequested?.Invoke(abilityIndex);
        }
    }
}