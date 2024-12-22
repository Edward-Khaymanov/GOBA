namespace GOBA.CORE
{
    public interface IAbilityProvider
    {
        public AbilityDefinition GetDefinition(string name);
        public AbilityBase GetAbilityTemplate(string prefabName);
    }
}