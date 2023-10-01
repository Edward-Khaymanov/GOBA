using System.Collections.Generic;
using UnityEngine;

namespace GOBA
{
    [CreateAssetMenu(fileName = "New Hero", menuName = "Create new hero", order = 51)]
    public class HeroAsset : UnitAsset
    {
        [field: SerializeField] public HeroAttributes BaseAttributes { get; set; }
        [field: SerializeField] public List<AbilityData> Abilities { get; set; }
    }
}