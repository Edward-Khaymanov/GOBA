using Cysharp.Threading.Tasks;
using GOBA.Assets._Project.Sources._Test;
using MapModCore;
using System;
using System.Collections.Generic;
using System.Threading;
using Unity.Netcode;
using UnityEngine;

namespace GOBA
{
    public class Hero : Unit
    {
        [SerializeField] private HeroView _view;
        //[SerializeField] private AbilityTargetSelector _abilityTargetSelector;


        private bool _isInitialized;
        private List<IAbility> _abilities;
        private IHeroInput _input;
        private CancellationTokenSource _abilityCancelationTokenSource;
        private HeroBaseState _currentState;
        private Dictionary<Type, HeroBaseState> _states;

        public AbilityActive CurrentAbitity { get; private set; }
        //public HeroAttributes Attributes { get; private set; }
        public HeroView View => _view;

        protected override void Update()
        {
            if (_isInitialized == false)
                return;

            _currentState.Update();
        }

        public void Initialize(int heroId, int teamId)
        {
            Init(heroId, teamId);
            InitAbilities(new[] { ABILITYLIST.GetAbility(1) });

            _isInitialized = true;
        }

        public void InitLocal(IHeroInput heroInput)
        {
            _input = heroInput;
            //_input.SetAbilityCallback(OnAbilityRequestedRpc);
            //_input.SetStopActionCallback(CancelActionRpc);
        }

        public void InitAbilities(IList<IAbility> abilities)
        {
            _abilities = new List<IAbility>();
            _abilities.AddRange(abilities);
        }

        public void Init(int heroId, int teamId)
        {
            var hero = UnitAssetProvider.GetHero(heroId);
            base.Init(heroId, teamId, hero.BaseStats);
            //Attributes = hero.BaseAttributes;
            _view.Init();
            _abilityCancelationTokenSource = new CancellationTokenSource();
            _states = new Dictionary<Type, HeroBaseState>()
            {
                { typeof(IdleState), new IdleState(this) },
                { typeof(MoveState), new MoveState(this, _navigationAgent) },
                { typeof(AttackState), new AttackState(this) },
                { typeof(AbilityCastingState), new AbilityCastingState(this) },
                { typeof(DeadState), new DeadState(this) },
            };
            _currentState = _states[typeof(IdleState)];
            _currentState.Enter();
        }

        public void SwitchState<T>() where T : HeroBaseState
        {
            var state = _states[typeof(T)];
            _currentState.Exist();
            MyLogger.Log($"Switching state from: {_currentState.GetType().Name} to: {state.GetType().Name}");
            state.Enter();
            _currentState = state;
        }

        public override void Move(Vector3 position)
        {
            MyLogger.Log("move");
            _currentState.Move(position);
        }

        public override void UseAbility(int abilityIndex)
        {
            UseAbility(abilityIndex, _abilityCancelationTokenSource.Token).Forget();
        }

        private async UniTaskVoid UseAbility(int abilityIndex, CancellationToken cancellationToken)
        {
            var abilityBase = _abilities[abilityIndex];
            var abilityActive = abilityBase as IAbilityActive;
            if (abilityActive.CanUse == false)
                return;

            //CurrentAbitity = abilityActive;
            await abilityActive.Activate(this);
            //var particle = _view.PlayParticle(abilityActive.ActivateParticle, transform.position, transform);
            //_view.OverrideAnimation(HeroAnimatorController.OverrideClips.AbilityActivation, abilityActive.ActivateAnimation);
            //_view.StartAnimatorTrigger(HeroAnimatorController.Params.AbilityActivationTrigger);
            //var targetPoint = new Vector3();


            //var castData = await _abilityTargetSelector.Select(this, new[] { TeamId }, abilityActive.Data.TargettingData, cancellationToken);

            //if (cancellationToken.IsCancellationRequested == false)
            //{
            //    UseAbilityServerRpc(abilityIndex, castData);
            //}

            //_abilityCancelationTokenSource = new CancellationTokenSource();
            //CurrentAbitity = null;
        }

        //[ServerRpc]
        //private void UseAbilityServerRpc(int index, AbilityCastData castData)
        //{
        //    UseAbilityServer(index, castData).Forget();
        //}

        //private async UniTaskVoid UseAbilityServer(int index, AbilityCastData castData)
        //{
        //    var abilityActive = _abilities[index] as IAbilityActive;

        //    if (abilityActive.CanUse == false)
        //        return;

        //    SwitchState<AbilityCastingState>();
        //    await abilityActive.Use(castData);
        //    SwitchState<IdleState>();

        //    //_view.OverrideAnimation(HeroAnimatorController.OverrideClips.AbilityUse, ability.UseAnimation);
        //    //_view.SetTrigger(HeroAnimatorController.Params.AbilityUseTrigger);
        //    //_view.SetTrigger(HeroAnimatorController.Params.AbilityUseEndTrigger);

        //}

        [Rpc(SendTo.Server)]
        public void CancelActionRpc()
        {
            _currentState.CancelAction();
        }

        [Rpc(SendTo.Server)]
        public void OnAbilityRequestedRpc(int abilityIndex)
        {
            //TryCancelAbility();
            var command = new UseAbilityCommand(this, abilityIndex);
            AddCommand(command);
            UseAbility(abilityIndex);
        }

        public void TryCancelAbility()
        {
            if (CurrentAbitity == null)
                return;

            CurrentAbitity.Deactivate();
            _abilityCancelationTokenSource.Cancel();
            _abilityCancelationTokenSource = new CancellationTokenSource();
            CurrentAbitity = null;
        }

        public override void AddCommand(ICommand command)
        {
            command.Execute();
        }
    }
}