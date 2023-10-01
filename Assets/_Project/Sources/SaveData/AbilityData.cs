using UnityEngine;

namespace GOBA
{
    [CreateAssetMenu(fileName = "AbilityData", menuName = "Create AbilityData", order = 51)]
    public class AbilityData : ScriptableObject
    {
        [SerializeField] public int Id;
        [SerializeField] public Sprite Icon;
        [SerializeReference] public AbilityBase Ability;
    }
}