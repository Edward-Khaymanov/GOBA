namespace GOBA.CORE
{
    public interface IUnit : IGameEntity, IMove, IAbilityOwner
    {
        public void CancelAction();
    }
}