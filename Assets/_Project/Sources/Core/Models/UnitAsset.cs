using UnityEngine;

namespace GOBA
{
    [CreateAssetMenu(fileName = "New Unit", menuName = "Create new Unit", order = 51)]
    public class UnitAsset : ScriptableObject
    {
        [field: SerializeField] public int Id { get; set; }
        [field: SerializeField] public string Name { get; set; }
        [field: SerializeField] public string IconKey { get; set; }
        [field: SerializeField] public string ModelKey { get; set; }
    }
}