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
        [SerializeField] private Collider _collider;
        [SerializeField] private NavMeshAgent _navigationAgent;

        //private float _manaBase = 100f;
        private NetworkVariable<bool> _isDead = new NetworkVariable<bool>();
        private NetworkVariable<float> _manaCurrent = new NetworkVariable<float>(400f);
        private NetworkVariable<float> _manaMax = new NetworkVariable<float>(1000f);
        private NetworkVariable<float> _healthCurrent = new NetworkVariable<float>(300f);
        private NetworkVariable<float> _healthMax = new NetworkVariable<float>(500f);
        private NetworkVariable<int> _teamId = new NetworkVariable<int>(-1);


        protected NavMeshAgent NavAgent => _navigationAgent;
        protected Collider Collider => _collider;

        protected virtual void Awake() { }

        protected virtual void Start() { }

        protected virtual void Update() { }

        protected virtual void OnEnable() { }

        protected virtual void OnDisable() { }

        public void Kill()
        {
            _isDead.Value = true;
            MyLogger.Log("died");
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

        public abstract void AddAbility(AbilityBase ability);

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

        public bool IsDead()
        {
            return _isDead.Value;
        }

        public float GetHeight()
        {
            return _navigationAgent.height;
        }

        public void SetTeam(int teamId)
        {
            _teamId.Value = teamId;
        }

        public int GetTeam()
        {
            return _teamId.Value;
        }

        public void SetHealth(float health)
        {
            health = Mathf.Max(health, 0);
            _healthCurrent.Value = health;
        }

        public void SetMaxHealth(float health)
        {
            health = Mathf.Max(health, 0);
            _healthMax.Value = health;
        }
    }
}