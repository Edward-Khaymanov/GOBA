using System;
using UnityEngine;

namespace GOBA
{
    [Serializable]
    public struct AbilityTargettingData
    {
        [field: SerializeField] public float CastRadius { get; private set; }
        [field: SerializeField] public float CastPointRadius { get; private set; }
        [field: SerializeField] public AbilityTargetType TargetType { get; private set; }
        [field: SerializeField] public AbilityTargettingType TargettingType { get; private set; }
        [field: SerializeField] public AbilityTargetTeam TargetTeam { get; private set; }
        [field: SerializeField] public bool IncludeSelf { get; private set; }
        [field: SerializeField] public int MaxTargets { get; private set; }
    }
}
