using Cysharp.Threading.Tasks;
using GOBA.CORE;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

namespace GOBA
{
    public class Hero : Unit
    {
        [SerializeField] private HeroView _view;

        private bool _isInitialized;
        private CancellationTokenSource _abilityCancelationTokenSource;

        public HeroView View => _view;

        private NetworkAbilityList _abilityList = new NetworkAbilityList();

        public void Initialize(int heroId, int teamId)
        {
            Init(heroId, teamId);

            _isInitialized = true;
        }

        protected override void Update()
        {
            base.Update();
            if (IsServer)
            {
                _abilityList.SetDirty(true);
            }
            DebugAbilityCooldown();
        }


        private void DebugAbilityCooldown()
        {
            var message = string.Empty;
            if (_abilityList == null)
            {
                message = "abilities is null";
            }
            if (_abilityList.Value.Count != 0)
            {
                message = $"{_abilityList.Value.FirstOrDefault().GetCooldownTimeRemaining()}";
            }
            Debug.Log(message);
        }


        private void DebugAbilityCount()
        {
            var message = string.Empty;

            if (_abilityList == null)
            {
                message = "abilities is null";
            }
            else if (_abilityList.Value == null)
            {
                message = "list is null";
            }
            else
            {
                message = $"{_abilityList.Value.Count}";
            }
            Debug.Log(message);
        }


        public override IList<Ability> GetAbilities()
        {
            return _abilityList.Value;
            //return _orderProperty.Values.Select(x => x.Value).Cast<IAbility>().ToList();
        }

        public void AddAbility(int abilityId, int orderId)
        {
            var abil = ABILITYLIST.GetAbility(abilityId);
            abil.SetOwner(this.Id);
            _abilityList.Add(abil);
        }

        public void RemoveAbility(int abilityId)
        {
            //var ability = _abilities.FirstOrDefault(x => x.Id == abilityId);
            //if (ability == null)
            //    return;

            //_abilities.Remove(ability);
        }

        public void Init(int heroId, int teamId)
        {
            var hero = UnitAssetProvider.GetHero(heroId);
            base.Init(heroId, teamId, hero.BaseStats);
            //Attributes = hero.BaseAttributes;
            _view.Init();
            _abilityCancelationTokenSource = new CancellationTokenSource();
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
            var ability = GetAbilities().FirstOrDefault(x => x.Id == abilityId);
            if (ability == null)
                return;

            if (ability.GetCooldownTimeRemaining() > 0)
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
    }
}