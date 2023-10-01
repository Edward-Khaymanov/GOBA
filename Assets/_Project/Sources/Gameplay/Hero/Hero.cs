using Cysharp.Threading.Tasks;
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
        [SerializeField] private AbilityTargetSelector _abilityTargetSelector;


        private bool _isInitialized;
        private List<AbilityBase> _abilities;
        private HeroInputController _inputController;
        private CancellationTokenSource _abilityCancelationTokenSource;
        private HeroBaseState _currentState;
        private Dictionary<Type, HeroBaseState> _states;


        public AbilityActive CurrentAbitity { get; private set; }
        public HeroAttributes Attributes { get; private set; }
        public HeroView View => _view;


        private void Update()
        {
            if (base.IsOwner == false || _isInitialized == false)
                return;

            _currentState.Update();
        }

        private void OnDisable()
        {
            _inputController.Disable();
        }

        public void Constructor(int heroId, int teamId)
        {
            Init(heroId, teamId);
        }

        [ClientRpc]
        public void ConstructorClientRPC(int heroId, int teamId, ClientRpcParams rpc)
        {
            if (base.IsServer == false)
                Init(heroId, teamId);

            if (base.IsOwner)
                LocalConstructor();
        }

        private void LocalConstructor()
        {
            _abilityTargetSelector.Constructor(Camera.main);
            _inputController.AbilityRequested += OnAbilityRequested;
            _inputController.Enable();
            _isInitialized = true;
        }

        private void Init(int heroId, int teamId)
        {
            var hero = UnitAssetProvider.GetHero(heroId);
            Init(heroId, teamId, hero.BaseStats);
            InitAbilities(hero.Abilities);
            Attributes = hero.BaseAttributes;

            _view.Init(hero.Skin);
            _inputController = new HeroInputController();
            _abilityCancelationTokenSource = new CancellationTokenSource();
            _states = new Dictionary<Type, HeroBaseState>()
            {
                { typeof(IdleState), new IdleState(this) },
                { typeof(MoveState), new MoveState(this, _navigationAgent, _inputController) },
                { typeof(AttackState), new AttackState(this) },
                { typeof(AbilityCastingState), new AbilityCastingState(this, _inputController) },
                { typeof(DeadState), new DeadState(this) },
            };
            _currentState = _states[typeof(IdleState)];
            _currentState.Enter();
        }

        private void InitAbilities(IList<AbilityData> abilitiesData)
        {
            _abilities = new List<AbilityBase>();
            foreach (var abilityData in abilitiesData)
            {
                _abilities.Add(abilityData.Ability);
            };
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

        public async UniTaskVoid UseAbility(int abilityIndex, CancellationToken cancellationToken)
        {
            var abilityBase = _abilities[abilityIndex];
            var abilityActive = abilityBase as AbilityActive;
            if (abilityActive.CanUse == false)
                return;

            CurrentAbitity = abilityActive;
            await abilityActive.Activate();
            //var particle = _view.PlayParticle(abilityActive.ActivateParticle, transform.position, transform);
            //_view.OverrideAnimation(HeroAnimatorController.OverrideClips.AbilityActivation, abilityActive.ActivateAnimation);
            //_view.StartAnimatorTrigger(HeroAnimatorController.Params.AbilityActivationTrigger);
            //var targetPoint = new Vector3();


            var castData = await _abilityTargetSelector.Select(this, new[] { TeamId }, abilityActive.Data.TargettingData, cancellationToken);

            if (cancellationToken.IsCancellationRequested == false)
            {
                UseAbilityServerRpc(abilityIndex, castData);
            }

            _abilityCancelationTokenSource = new CancellationTokenSource();
            CurrentAbitity = null;
        }

        [ServerRpc]
        private void UseAbilityServerRpc(int index, AbilityCastData castData)
        {
            UseAbilityServer(index, castData).Forget();
        }

        private async UniTaskVoid UseAbilityServer(int index, AbilityCastData castData)
        {
            var abilityActive = _abilities[index] as AbilityActive;

            if (abilityActive.CanUse == false)
                return;

            SwitchState<AbilityCastingState>();
            await abilityActive.Use(castData);
            SwitchState<IdleState>();

            //_view.OverrideAnimation(HeroAnimatorController.OverrideClips.AbilityUse, ability.UseAnimation);
            //_view.SetTrigger(HeroAnimatorController.Params.AbilityUseTrigger);
            //_view.SetTrigger(HeroAnimatorController.Params.AbilityUseEndTrigger);

        }



        private void OnAbilityRequested(int abilityIndex)
        {
            CancelAbility();
            UseAbility(abilityIndex - 1, _abilityCancelationTokenSource.Token).Forget();//////////////
        }

        public void CancelAbility()
        {
            if (CurrentAbitity == null)
                return;

            CurrentAbitity.Deactivate();
            _abilityCancelationTokenSource.Cancel();
            _abilityCancelationTokenSource = new CancellationTokenSource();
            CurrentAbitity = null;
        }
    }
}