using System.Collections.Generic;

namespace GOBA.CORE
{
    public interface IUnit : IGameEntity, IMove
    {
        public IList<Ability> GetAbilities();
        public void UseAbility(int abilityId, AbilityCastData castData);
        public void CancelAction();
    }
}