using UnityEngine;

namespace GOBA
{
    public class AbilityCastingState : HeroBaseState
    {
        public AbilityCastingState(Hero hero) : base(hero)
        {

        }

        public override void CancelAction()
        {
            StopCasting();
        }

        public override void Enter()
        {
            _hero.View.PlayState(HeroAnimatorController.States.AbilityUse);
        }

        public override void Exist()
        {

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