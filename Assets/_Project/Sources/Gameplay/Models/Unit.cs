using GOBA.CORE;
using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

namespace GOBA
{
    [SelectionBase]
    public abstract class Unit : GameEntity, IUnit
    {
        [SerializeField] private UnitCanvas _unitCanvas;
        [SerializeField] private NavMeshAgent _navigationAgent;

        //private float _manaBase = 100f;
        private NetworkVariable<bool> _isDead = new NetworkVariable<bool>();
        private NetworkVariable<float> _manaCurrent = new NetworkVariable<float>(400f);
        private NetworkVariable<float> _manaMax = new NetworkVariable<float>(1000f);
        private NetworkVariable<float> _healthCurrent = new NetworkVariable<float>(300f);
        private NetworkVariable<float> _healthMax = new NetworkVariable<float>(500f);
        private NetworkVariable<int> _teamId = new NetworkVariable<int>(-1);


        protected NavMeshAgent NavAgent => _navigationAgent;

        //public event Action OnDamageTaken;
        //public event Action<float, float> OnHealfChanged;

        //public int AssetId { get; private set; }
        //public int TeamId { get; private set; }
        //public UnitStats Stats { get; private set; }

        protected virtual void Awake() { }

        protected virtual void Start() { }

        protected virtual void Update() { }

        protected virtual void OnEnable()
        {
            //OnDamageTaken += TryKill;
        }

        protected virtual void OnDisable()
        {
            //OnDamageTaken -= TryKill;
            //Stats.Healf.CurrentChanged -= (current) => _unitCanvas.OnHealfChanged(current, Stats.Healf.Max);
            //Stats.Healf.MaxChanged -= (max) => _unitCanvas.OnHealfChanged(Stats.Healf.Current, max);
        }

        //protected void Init(int assetId, int teamId, UnitStats stats)
        //{
        //    AssetId = assetId;
        //    TeamId = teamId;
        //    Stats = stats;

        //    //Stats.Healf.CurrentChanged += (current) => _unitCanvas.OnHealfChanged(current, Stats.Healf.Max);
        //    //Stats.Healf.MaxChanged += (max) => _unitCanvas.OnHealfChanged(Stats.Healf.Current, max);
        //}

        public virtual void TakeDamage(DamageType damageType, float damage)
        {
            //var reducedDamage = Stats.Armor.GetReducedDamage(damageType, damage);
            //Stats.Healf.Remove(reducedDamage);
            //OnDamageTaken?.Invoke();
        }

        protected virtual void TryKill()
        {
            //if (Stats.Healf.Current > 0)
            //    return;

            MyLogger.Log("i am dead", LogLevel.Warning);
        }

        public virtual void MoveTo(Vector3 position)
        {
            var path = new NavMeshPath();
            _navigationAgent.CalculatePath(position, path);
            _navigationAgent.SetPath(path);
        }

        public abstract IList<AbilityBase> GetAbilities();
        public abstract void UseAbility(int abilityId, AbilityCastData castData);
        public abstract void CancelAction();

        public void SpendMana(float amount, AbilityBase ability)
        {
            _manaCurrent.Value = Mathf.Clamp(_manaCurrent.Value - Mathf.Abs(amount), 0, _manaMax.Value);
        }

        public float GetHealth()
        {
            return _healthCurrent.Value;
        }

        public float GetMaxHealth()
        {
            return _healthMax.Value;
        }

        public float GetMana()
        {
            return _manaCurrent.Value;
        }

        public float GetMaxMana()
        {
            return _manaMax.Value;
        }

        public void SetTeam(int teamId)
        {
            _teamId.Value = teamId;
        }

        public int GetTeam()
        {
            return _teamId.Value;
        }
    }
}