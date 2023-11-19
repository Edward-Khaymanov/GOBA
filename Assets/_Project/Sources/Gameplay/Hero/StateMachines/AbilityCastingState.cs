using UnityEngine;

namespace GOBA
{
    public class AbilityCastingState : HeroBaseState
    {
        private readonly IHeroInput _inputController;

        public AbilityCastingState(Hero hero, IHeroInput inputController) : base(hero)
        {
            _inputController = inputController;
        }

        public override void Enter()
        {
            _hero.View.SetTrigger(HeroAnimatorController.Params.AbilityUseTrigger);
            _inputController.StopActionRequsted += StopCasting;
        }

        public override void Exist()
        {
            _inputController.StopActionRequsted -= StopCasting;
        }

        public override void Move(Vector3 position)
        {
        }

        public override void Update()
        {
        }

        private void StopCasting()
        {
            _hero.TryCancelAbility();
            _hero.SwitchState<IdleState>();
        }
    }
}