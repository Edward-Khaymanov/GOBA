using UnityEngine;

namespace GOBA
{
    [CreateAssetMenu(fileName = "New Unit", menuName = "Create new Unit", order = 51)]
    public class UnitAsset : ScriptableObject
    {
        [field: SerializeField] public int Id { get; set; }
        [field: SerializeField] public string Name { get; set; }
        [field: SerializeField] public Sprite Icon { get; set; }
        [field: SerializeField] public UnitView Skin { get; set; }
        [field: SerializeField] public UnitStats BaseStats { get; set; }
    }
}