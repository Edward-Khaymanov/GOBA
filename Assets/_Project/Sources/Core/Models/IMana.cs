namespace GOBA.CORE
{
    public interface IMana
    {
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
        public float GetMana();
        public float GetMaxMana();
        public void SetMana(float amount);
        public void SetMaxMana(float amount);
    }
}