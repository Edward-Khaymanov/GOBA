using System;
using System.Collections.Generic;
using UnityEngine;

namespace GOBA
{
    [Serializable]
    public struct AbilityActiveData
    {
        [field: SerializeField] public CostType CostType { get; private set; }
        [field: SerializeField] public float Cost { get; private set; }
        [field: SerializeField] public float CastTimeInSeconds { get; private set; }
        [field: SerializeField] public float CooldownInSeconds { get; private set; }
        [field: SerializeField] public AbilityTargettingData TargettingData { get; private set; }
        //[field: SerializeField] public IEnumerable<IEffect> AttachableEffects { get; private set; }
    }
}