using System.Collections.Generic;

namespace GOBA.CORE
{
    public interface IAbilityOwner
    {
        public IList<Ability> GetAbilities();
        public void UseAbility(int abilityId, AbilityCastData castData);
    }
}