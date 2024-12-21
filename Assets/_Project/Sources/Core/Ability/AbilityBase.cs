using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using Unity.Netcode;
using UnityEngine;

namespace GOBA.CORE
{
    public abstract class AbilityBase : GameEntity
    {
        protected IProjectileManager ProjectileManager;
        protected IParticleManager ParticleManager;

        private IEntityManager _entityManager;
        private AbilityCastData _castData;
        private CancellationTokenSource _cooldownCancellationSource;

        private NetworkVariable<int> _abilityId = new NetworkVariable<int>();
        private NetworkVariable<int> _ownerEntityId = new NetworkVariable<int>();
        private NetworkVariable<int> _level = new NetworkVariable<int>();
        private NetworkVariable<float> _cooldown = new NetworkVariable<float>();
        private NetworkVariable<AbilityDefinition> _abilityDefinition = new NetworkVariable<AbilityDefinition>();

        public int AbilityId => _abilityId.Value;

        public void SetDependencies(IProjectileManager projectileManager, IParticleManager particleManager, IEntityManager entityManager)
        {
            ProjectileManager = projectileManager;
            ParticleManager = particleManager;
            _entityManager = entityManager;
        }

        public virtual void Initialize(AbilityDefinition definition)
        {
            _abilityId.Value = definition.Id;
            _abilityDefinition.Value = definition;
        }

        public async UniTask CastAbility(AbilityCastData castData)
        {
            SetCastData(castData);
            var canBeUsed = CanBeUsed();
            if (canBeUsed == false)
                return;

            await OnAbilityPhaseStart();
            //включаем анимацию (может и не отсюда хз)
            var castTime = GetCastTime();
            //////////////////////////////////////ВСЕ ЧТО ДАЛЬШЕ НАДО КАК ТО ПРЕРВАТЬ, ЕСЛИ НАС ПРЕРВАЛИ
            canBeUsed = CanBeUsed();
            if (canBeUsed == false)
            {
                OnAbilityPhaseInterrupted();
                return;
            }
            await UniTask.Delay(TimeSpan.FromSeconds(castTime));
            //это не прерывается
            PayCost();
            var cooldown = GetCooldown();
            StartCooldown(cooldown);
            await OnSpellStart();
        }

        public AbilityBehaviour GetBehaviour()
        {
            return GetDefinitionData<AbilityBehaviour>("AbilityBehaviour");
        }

        public AbilityUnitTargetType GetTargetType()
        {
            return GetDefinitionData<AbilityUnitTargetType>("AbilityUnitTargetType");
        }

        public AbilityUnitTargetTeam GetTargetTeam()
        {
            return GetDefinitionData<AbilityUnitTargetTeam>("AbilityUnitTargetTeam");
        }

        public IUnit GetOwner()
        {
            return _entityManager.GetEntity(_ownerEntityId.Value) as IUnit;
        }

        public void SetOwner(int ownerEntityId)
        {
            _ownerEntityId.Value = ownerEntityId;
        }

        public float GetCastTime()
        {
            return GetDefinitionData<float>("CastTime");
        }

        public IUnit GetCastTarget()
        {
            return _entityManager.GetUnit(_castData.TargetEntityId);
        }

        public Vector3 GetCastPoint()
        {
            return _castData.CastPoint;
        }

        #region COOLDOWN

        public float GetCooldownTimeRemaining()
        {
            return _cooldown.Value;
        }

        public float GetCooldown(int level = -1)
        {
            var cost = GetDefinitionData<List<float>>("Cooldown");
            if (level == -1)
                level = GetLevel();

            return cost[level - 1];
        }

        public void StartCooldown(float cooldown)
        {
            _cooldownCancellationSource = new CancellationTokenSource();
            StartCooldownAsync(cooldown, _cooldownCancellationSource.Token).Forget();
        }

        public void EndCooldown()
        {
            _cooldownCancellationSource?.Cancel();
            _cooldown.Value = 0f;
        }

        #endregion

        #region COST

        public virtual void PayCost()
        {
            var caster = GetOwner();
            var cost = GetCost();
            caster.SpendMana(cost, this);
        }

        public float GetCost(int level = -1)
        {
            var cost = GetDefinitionData<List<float>>("Cost");

            if (level == -1)
                level = GetLevel();

            return cost[level - 1];
        }

        #endregion

        #region LEVEL

        public int GetLevel()
        {
            return _level.Value;
        }

        public void SetLevel(int level)
        {
            _level.Value = Mathf.Clamp(level, 0, GetMaxLevel());
        }

        public int GetMaxLevel()
        {
            return GetDefinitionData<int>("MaxLevel");
        }

        #endregion

        #region EVENTS

        public virtual async UniTask<bool> OnProjectileHit() { return false; }
        protected virtual async UniTask OnAbilityPhaseStart() { }
        protected virtual async UniTask OnAbilityPhaseInterrupted() { }
        protected virtual async UniTask OnSpellStart() { }
        protected virtual async UniTask OnUpgrade() { }
        protected virtual async UniTask OnOwnerSpawned() { }
        protected virtual async UniTask OnOwnerDied() { }
        protected virtual async UniTask OnHeroLevelUp() { }
        //protected abstract void OnSync<T>(BufferSerializer<T> serializer) where T : IReaderWriter;

        #endregion

        protected T GetDefinitionData<T>(string key)
        {
            var result = default(T);
            var type = typeof(T);

            if (type.IsEnum)
            {
                var intValue = _abilityDefinition.Value.Data.Value<int>(key);
                result = (T)Enum.ToObject(type, intValue);
            }
            else if (type.IsArray || type.IsIEnumerable())
            {
                result = _abilityDefinition.Value.Data.SelectToken(key).ToObject<T>();
            }
            else
            {
                result = _abilityDefinition.Value.Data.Value<T>(key);
            }

            return result;
        }

        private bool CanBeUsed()
        {
            var result = true;
            var caster = GetOwner();

            if (GetCooldownTimeRemaining() > 0)
            {
                Debug.LogWarning("COOLDOWN");
                result = false;
            }

            if (GetCost() > caster.GetMana())
            {
                Debug.LogWarning("MANA");
                result = false;
            }
            return result;
        }


        private async UniTaskVoid StartCooldownAsync(float cooldown, CancellationToken cancellationToken)
        {
            var newCooldown = cooldown;
            _cooldown.Value = newCooldown;

            while (_cooldown.Value > 0f && cancellationToken.IsCancellationRequested == false)
            {
                await UniTask.NextFrame();
                if (cancellationToken.IsCancellationRequested)
                    break;

                newCooldown -= Time.deltaTime;
                if (newCooldown < 0f)
                    newCooldown = 0f;

                _cooldown.Value = newCooldown;
            }
        }


        private void SetCastData(AbilityCastData castData)
        {
            _castData = castData;
        }

        private void ClearCastData()
        {
            _castData = default;
        }

        protected sealed override void OnSynchronize<T>(ref BufferSerializer<T> serializer)
        {
            base.OnSynchronize(ref serializer);
        }
    }
}