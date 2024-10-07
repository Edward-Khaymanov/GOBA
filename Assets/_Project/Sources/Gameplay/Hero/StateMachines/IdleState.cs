//using UnityEngine;

//namespace GOBA
//{
//    public class IdleState : HeroBaseState
//    {

//        public IdleState(Hero hero) : base(hero)
//        {
//        }

//        public override void CancelAction()
//        {

//        }

//        public override void Enter()
//        {
//            _hero.View.PlayState(HeroAnimatorController.States.Idle);
//        }

//        public override void Exist()
//        {
//        }

//        public override void Move(Vector3 position)
//        {
//            _hero.SwitchState<MoveState>();
//            _hero.Move(position);
//        }

//        public override void Update()
//        {

//        }


//    }
//}