using System;
using System.Collections.Generic;
using UnityEngine;

namespace GOBA
{
    [Serializable]
    public class Armor
    {
        private float _reduceMultiplier;
        private Dictionary<DamageType, Func<float, float>> _actions;

        public Armor()
        {
            _reduceMultiplier = CONSTANTS.ARMOR_REDUCE_MULTIPLIER;
            _actions = new Dictionary<DamageType, Func<float, float>>()
            {
                { DamageType.Pure, ReducePureDamage },
                { DamageType.Physical, ReducePhysicalDamage },
                { DamageType.Magical, ReduceMagicDamage }
            };
        }

        public Armor(float amount) : this()
        {
            Amount = amount;
        }

        [field: SerializeField] public float Amount { get; private set; }
        public float Resist => GetResist();

        public void Add(float amount)
        {
            if (amount <= 0)
                return;

            Amount += amount;
        }

        public void Remove(float amount)
        {
            if (amount <= 0)
                return;

            Amount -= amount;
        }

        public float GetReducedDamage(DamageType damageType, float damage)
        {
            return _actions[damageType](damage);
        }

        private float ReducePureDamage(float damage)
        {
            return damage;
        }

        private float ReducePhysicalDamage(float damage)
        {
            return damage * (1 - Resist);
        }

        private float ReduceMagicDamage(float damage)
        {
            return damage * (1 - Resist);
        }

        private float GetResist()
        {
            var multiplier = _reduceMultiplier * Amount;
            return multiplier / (1 + multiplier);
        }
    }
}