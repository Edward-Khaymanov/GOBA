using System.Collections;
using System.Collections.Generic;

namespace GOBA.CORE
{
    public interface IEntityManager
    {
        public int GetAvailableEntityId();
        public void AddEntity(IGameEntity entity);
        public void RemoveEntity(IGameEntity entity);
        public void RemoveEntity(int entityId);
        public IGameEntity GetEntity(int entityId);
        public IEnumerable<IGameEntity> GetEntities(IEnumerable<int> entitiesId);
        public void Reset();
    }
}