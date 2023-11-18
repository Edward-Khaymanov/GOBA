using System;
using UnityEngine;

namespace GOBA
{
    [Serializable]
    public class HeroAttributes
    {
        public HeroAttributes()
        {

        }

        public HeroAttributes(float agility, float strength, float intelligence)
        {
            Agility = agility;
            Strength = strength;
            Intelligence = intelligence;
        }

        [field: SerializeField] public float Agility { get; private set; }
        [field: SerializeField] public float Strength { get; private set; }
        [field: SerializeField] public float Intelligence { get; private set; }
    }
}