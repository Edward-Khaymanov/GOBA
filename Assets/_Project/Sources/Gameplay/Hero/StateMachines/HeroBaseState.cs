using UnityEngine;

namespace GOBA
{
    public abstract class HeroBaseState : BaseState
    {
        protected Hero _hero;

        protected HeroBaseState(Hero hero)
        {
            _hero = hero;
        }

        public abstract void Move(Vector3 position);
        public abstract void CancelAction();
    }
}