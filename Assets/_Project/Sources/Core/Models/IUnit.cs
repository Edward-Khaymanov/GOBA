using System.Collections.Generic;

namespace GOBA.CORE
{
    public interface IUnit : IGameEntity, IMove, ITeam, IMana, IHealth
    {
        public void CancelAction();
        public IList<AbilityBase> GetAbilities();
        public void UseAbility(int abilityId, AbilityCastData castData);
        public void AddAbility(AbilityBase ability);
        public float GetHeight();//временный метод для правильного отображения баров над юнитом
    }
}