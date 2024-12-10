using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using Unity.Netcode;
using UnityEngine;

namespace GOBA.CORE
{
    public abstract class AbilityBase : GameEntity/*, INetworkSerializable, IEquatable<Ability>*/
    {
        private int _abilityId;
        private AbilityDefinition _abilityDefinition;
        private int _ownerEntityId;
        private int _level;
        private float _cooldown;

        private CancellationTokenSource _cooldownCancellationSource;


        protected IProjectileManager ProjectileManager;
        protected IParticleManager ParticleManager;




        public int AbilityId => _abilityId;

        //public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        //{
        //    serializer.SerializeValue(ref _abilityId);
        //    serializer.SerializeValue(ref _ownerEntityId);
        //    OnSync(serializer);
        //}

        //public bool Equals(Ability other)
        //{
        //    return _abilityId == other._abilityId;
        //}

        public void SetDependencies(IProjectileManager projectileManager, IParticleManager particleManager)
        {
            ProjectileManager = projectileManager;
            ParticleManager = particleManager;
        }

        public virtual void Initialize(AbilityDefinition definition)
        {
            _abilityId = definition.Id;
            _abilityDefinition = definition;
        }

        public async UniTask CastAbility(AbilityCastData castData)
        {
            await OnAbilityPhaseStart(castData);
            //включаем анимацию (может и не отсюда хз)
            var castTime = GetCastTime();
            //////////////////////////////////////ВСЕ ЧТО ДАЛЬШЕ НАДО КАК ТО ПРЕРВАТЬ, ЕСЛИ НАС ПРЕРВАЛИ
            await UniTask.Delay(TimeSpan.FromSeconds(castTime));
            //это не прерывается
            var caster = GetOwner();
            if (GetCost() > caster.GetMana())
            {
                Debug.LogWarning("НЕТ МАНЫ");
                OnAbilityPhaseInterrupted(castData);
                return;
            }
            PayCost();
            var cooldown = GetCooldown();
            StartCooldown(cooldown);
            await OnSpellStart(castData);
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
            return DIContainer.EntityManager.GetEntity(_ownerEntityId) as IUnit;
        }

        public void SetOwner(int ownerEntityId)
        {
            _ownerEntityId = ownerEntityId;
        }

        public float GetCastTime()
        {
            return GetDefinitionData<float>("CastTime");
        }

        #region COOLDOWN

        public float GetCooldownTimeRemaining()
        {
            return _cooldown;
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
            _cooldown = 0f;
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
            return _level;
        }

        public void SetLevel(int level)
        {
            _level = Math.Clamp(level, 0, GetMaxLevel());
        }

        public int GetMaxLevel()
        {
            return GetDefinitionData<int>("MaxLevel");
        }

        #endregion

        #region EVENTS

        public virtual async UniTask<bool> OnProjectileHit() { return false; }
        protected virtual async UniTask OnAbilityPhaseStart(AbilityCastData castData) { }
        protected virtual async UniTask OnAbilityPhaseInterrupted(AbilityCastData castData) { }
        protected virtual async UniTask OnSpellStart(AbilityCastData castData) => throw new NotImplementedException();
        protected virtual async UniTask OnUpgrade() => throw new NotImplementedException();
        protected virtual async UniTask OnOwnerSpawned() => throw new NotImplementedException();
        protected virtual async UniTask OnOwnerDied() => throw new NotImplementedException();
        protected virtual async UniTask OnHeroLevelUp() => throw new NotImplementedException();
        protected abstract void OnSync<T>(BufferSerializer<T> serializer) where T : IReaderWriter;

        #endregion

        protected T GetDefinitionData<T>(string key)
        {
            var result = default(T);
            var type = typeof(T);

            if (type.IsEnum)
            {
                var intValue = _abilityDefinition.Data.Value<int>(key);
                result = (T)Enum.ToObject(type, intValue);
            }
            else if (type.IsArray || type.IsIEnumerable())
            {
                result = _abilityDefinition.Data.SelectToken(key).ToObject<T>();
            }
            else
            {
                result = _abilityDefinition.Data.Value<T>(key);
            }

            return result;
        }

        private async UniTaskVoid StartCooldownAsync(float cooldown, CancellationToken cancellationToken)
        {
            var newCooldown = cooldown;
            _cooldown = newCooldown;

            while (_cooldown > 0f && cancellationToken.IsCancellationRequested == false)
            {
                await UniTask.NextFrame();
                if (cancellationToken.IsCancellationRequested)
                    break;

                newCooldown -= Time.deltaTime;
                if (newCooldown < 0f)
                    newCooldown = 0f;

                _cooldown = newCooldown;
            }
        }
    }
}