using UnityEngine;
using UnityEngine.AI;

namespace GOBA
{
    public class MoveState : HeroBaseState
    {
        private bool _isMoving;
        private readonly NavMeshAgent _navMeshAgent;
        private readonly IHeroInput _inputController;

        public MoveState(Hero hero, NavMeshAgent navMeshAgent, IHeroInput inputController) : base(hero)
        {
            _navMeshAgent = navMeshAgent;
            _inputController = inputController;
        }

        public override void Enter()
        {
            _inputController.StopActionRequsted += StopMove;
        }

        public override void Exist()
        {
            _isMoving = false;
            _navMeshAgent.ResetPath();
            _inputController.StopActionRequsted -= StopMove;
        }

        public override void Move(Vector3 position)
        {
            //_navMeshAgent.SetDestination(position); асинхронно, но более точно
            var path = new NavMeshPath();
            _navMeshAgent.CalculatePath(position, path);
            _navMeshAgent.SetPath(path);
            _hero.View.SetTrigger(HeroAnimatorController.Params.RunTrigger);
            _isMoving = true;
        }

        public override void Update()
        {
            if (_isMoving == false)
                return;

            if (_navMeshAgent.pathPending == false && _navMeshAgent.hasPath == false)
            {
                StopMove();
            }
        }

        private void StopMove()
        {
            _hero.SwitchState<IdleState>();
        }
    }
}