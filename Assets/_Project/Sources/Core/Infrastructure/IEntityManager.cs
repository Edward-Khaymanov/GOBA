namespace GOBA.CORE
{
    public interface IEntityManager
    {
        public int GetAvailableEntityId();
        public void AddEntity(IGameEntity entity);
        public void RemoveEntity(IGameEntity entity);
        public void RemoveEntity(int entityId);
        public IGameEntity GetEntity(int entityId);
        public void Reset();
    }
}