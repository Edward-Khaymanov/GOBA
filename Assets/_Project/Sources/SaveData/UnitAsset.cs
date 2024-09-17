using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GOBA
{
    [CreateAssetMenu(fileName = "New Unit", menuName = "Create new Unit", order = 51)]
    public class UnitAsset : ScriptableObject
    {
        [field: SerializeField] public int Id { get; set; }
        [field: SerializeField] public string Name { get; set; }
        [field: SerializeField] public Sprite Icon { get; set; }
        [field: SerializeField] public UnitModel Model { get; set; }
        [field: SerializeField] public UnitStats BaseStats { get; set; }
    }
}