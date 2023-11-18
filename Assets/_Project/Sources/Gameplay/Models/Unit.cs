using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

namespace GOBA
{
    [SelectionBase]
    public abstract class Unit : NetworkBehaviour, IMove, IDamage, IHeal
    {
        [SerializeField] private UnitCanvas _unitCanvas;

        protected NavMeshAgent _navigationAgent;


        public event Action OnDamageTaken;
        public event Action<float, float> OnHealfChanged;

        public int AssetId { get; private set; }
        public int TeamId { get; private set; }
        public UnitStats Stats { get; private set; }
        public bool IsNeutral => TeamId == CONSTANTS.NEUTRAL_TEAM_ID;


        protected virtual void Awake()
        {
            _navigationAgent = GetComponent<NavMeshAgent>();
        }

        private void OnEnable()
        {
            OnDamageTaken += TryKill;
        }

        private void OnDisable()
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

        public void TakeHeal(float amount)
        {
            Stats.Healf.Add(amount);
        }
    }
}