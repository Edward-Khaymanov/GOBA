using MapModCore;
using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

namespace GOBA
{
    [SelectionBase]
    public abstract class Unit : NetworkBehaviour, IUnit
    {
        [SerializeField] private UnitCanvas _unitCanvas;

        protected NavMeshAgent _navigationAgent;

        public event Action OnDamageTaken;
        public event Action<float, float> OnHealfChanged;

        public int AssetId { get; private set; }
        public int TeamId { get; private set; }
        public UnitStats Stats { get; private set; }
        public bool IsNeutral => TeamId == CONSTANTS.NEUTRAL_TEAM_ID;

        public Transform Transform => transform;
        public NetworkBehaviour NetworkBehaviour => this;

        protected virtual void Awake()
        {
            _navigationAgent = GetComponent<NavMeshAgent>();
        }

        protected virtual void Update()
        {

        }

        protected virtual void OnEnable()
        {
            OnDamageTaken += TryKill;
        }

        protected virtual void OnDisable()
        {
            OnDamageTaken -= TryKill;
            Stats.Healf.CurrentChanged -= (current) => _unitCanvas.OnHealfChanged(current, Stats.Healf.Max);
            Stats.Healf.MaxChanged -= (max) => _unitCanvas.OnHealfChanged(Stats.Healf.Current, max);
        }

        protected void Init(int assetId, int teamId, UnitStats stats)
        {
            AssetId = assetId;
            TeamId = teamId;
            Stats = stats;

            Stats.Healf.CurrentChanged += (current) => _unitCanvas.OnHealfChanged(current, Stats.Healf.Max);
            Stats.Healf.MaxChanged += (max) => _unitCanvas.OnHealfChanged(Stats.Healf.Current, max);
        }


        public virtual void Move(Vector3 position)
        {
            if (IsOwner == false)
                return;

            _navigationAgent.SetDestination(position);
        }

        public virtual void TakeDamage(DamageType damageType, float damage)
        {
            var reducedDamage = Stats.Armor.GetReducedDamage(damageType, damage);
            Stats.Healf.Remove(reducedDamage);
            OnDamageTaken?.Invoke();
        }

        protected virtual void TryKill()
        {
            if (Stats.Healf.Current > 0)
                return;

            MyLogger.Log("i am dead", LogLevel.Warning);
        }

        public virtual void EnableInput()
        {
        }

        public virtual void DisableInput()
        {
        }

        public virtual void InitBindings()
        {

        }

        public virtual void MoveTo(Vector3 position)
        {

        }

        public virtual void UseAbility(int abilityIndex)
        {

        }

        public abstract void AddCommand(ICommand command);
    }
}