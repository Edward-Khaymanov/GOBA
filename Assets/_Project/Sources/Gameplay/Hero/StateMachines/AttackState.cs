using UnityEngine;

namespace GOBA
{
    public class AttackState : HeroBaseState
    {
        public AttackState(Hero hero) : base(hero)
        {
        }

        public override void Enter()
        {

        }

        public override void Exist()
        {

        }

        public override void Move(Vector3 position)
        {
            _hero.SwitchState<MoveState>();
            _hero.Move(position);
        }

        public override void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                _hero.SwitchState<IdleState>();
            }
        }
    }
}