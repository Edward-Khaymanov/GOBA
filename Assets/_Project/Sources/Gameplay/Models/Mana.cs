using System;
using UnityEngine;

namespace GOBA
{
    [Serializable]
    public class Mana
    {
        public Mana()
        {

        }

        [field: SerializeField] public float Current { get; private set; }
        [field: SerializeField] public float Max { get; private set; }
    }
}