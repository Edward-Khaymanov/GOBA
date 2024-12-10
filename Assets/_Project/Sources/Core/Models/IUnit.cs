using System.Collections.Generic;

namespace GOBA.CORE
{
    public interface IUnit : IGameEntity, IMove
    {
        public void CancelAction();


        public IList<AbilityBase> GetAbilities();
        public void UseAbility(int abilityId, AbilityCastData castData);
        public void AddAbility(AbilityBase ability);

        /// <summary>
        /// потратил ману на абилку
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="ability"></param>
        public void SpendMana(float amount, AbilityBase ability);
        ///// <summary>
        ///// потерял ману (по типу никса 2 скилла)
        ///// </summary>
        ///// <param name="amount"></param>
        //public void ReduceMana(float amount);
        //public void GiveMana(float amount);
        //public void GetMaxMana();
        public float GetMana();



    }
}