//using UnityEngine;
//using UnityEngine.AI;

//namespace GOBA
//{
//    public class MoveState : HeroBaseState
//    {
//        private bool _isMoving;
//        private readonly NavMeshAgent _navMeshAgent;

//        public MoveState(Hero hero, NavMeshAgent navMeshAgent) : base(hero)
//        {
//            _navMeshAgent = navMeshAgent;
//        }

//        public override void Enter()
//        {

//        }

//        public override void Exist()
//        {
//            _isMoving = false;
//            _navMeshAgent.ResetPath();
//        }

//        public override void Move(Vector3 position)
//        {
//            //_navMeshAgent.SetDestination(position); асинхронно, но более точно
//            var path = new NavMeshPath();
//            _navMeshAgent.CalculatePath(position, path);
//            _navMeshAgent.SetPath(path);
//            _hero.View.PlayState(HeroAnimatorController.States.Move);
//            _isMoving = true;
//        }

//        public override void Update()
//        {
//            if (_isMoving == false)
//                return;

//            if (_navMeshAgent.pathPending == false && _navMeshAgent.hasPath == false)
//            {
//                StopMove();
//            }
//        }

//        private void StopMove()
//        {
//            _hero.SwitchState<IdleState>();
//        }

//        public override void CancelAction()
//        {
//            StopMove();
//        }
//    }
//}