using Cysharp.Threading.Tasks;
using GOBA.CORE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Unity.Netcode;
using UnityEngine;

namespace GOBA
{
    public class Hero : Unit
    {
        [SerializeField] private HeroView _view;

        private CancellationTokenSource _abilityCancelationTokenSource;
        private NetworkList<int> _abilityList;

        public HeroView View => _view;

        protected override void Awake()
        {
            base.Awake();
            _abilityList = new NetworkList<int>();
        }

        public void Initialize()
        {
            _view.Init();
            _abilityCancelationTokenSource = new CancellationTokenSource();
            NavAgent.enabled = true;
            Collider.enabled = true;
        }

        public override IList<AbilityBase> GetAbilities()
        {
            var abiltiesEntity = DIContainer.EntityManager.GetEntities(_abilityList.GetValues());
            return abiltiesEntity.Cast<AbilityBase>().ToList();
        }

        public void RemoveAbility(int abilityId)
        {
            throw new NotImplementedException();
        }

        public override void MoveTo(Vector3 position)
        {
            MyLogger.Log("move");
            base.MoveTo(position);
            View.PlayState(HeroAnimatorController.States.Move);
        }

        public override void UseAbility(int abilityId, AbilityCastData castData)
        {
            UseAbility(abilityId, castData, _abilityCancelationTokenSource.Token).Forget();
        }

        public override void CancelAction()
        {
            _abilityCancelationTokenSource.Cancel();
            _abilityCancelationTokenSource = new CancellationTokenSource();
        }

        private async UniTaskVoid UseAbility(int abilityId, AbilityCastData castData, CancellationToken cancellationToken)
        {
            var ability = GetAbilities().FirstOrDefault(x => x.AbilityId == abilityId);
            if (ability == null)
                return;

            //CurrentAbitity = abilityActive;
            ability.CastAbility(castData);
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

        public override void AddAbility(AbilityBase ability)
        {
            _abilityList.Add(ability.EntityId);
        }
    }
}