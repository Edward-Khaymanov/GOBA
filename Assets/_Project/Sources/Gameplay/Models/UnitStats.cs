using System;
using UnityEngine;

namespace GOBA
{
    [Serializable]
    public class UnitStats
    {
        public UnitStats()
        {

        }

        public UnitStats(Healf healf, Armor armor, Mana mana, float attackRange)
        {
            Healf = healf;
            Armor = armor;
            Mana = mana;
            AttackRange = attackRange;
        }

        [field: SerializeField] public Healf Healf { get; private set; }
        [field: SerializeField] public Armor Armor { get; private set; }
        [field: SerializeField] public Mana Mana { get; private set; }
        [field: SerializeField] public float AttackRange { get; private set; }
        [field: SerializeField] public int BaseAttackSpeed { get; protected set; }

    }
}