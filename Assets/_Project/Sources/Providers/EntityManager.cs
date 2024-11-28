using MapModCore;
using System.Collections.Generic;
using System.Linq;

namespace GOBA
{
    public class EntityManager : IEntityManager
    {
        private HashSet<IGameEntity> _entities;
        private int _currentAvailableEntityId;

        public EntityManager()
        {
            Reset();
        }

        public int GetAvailableEntityId()
        {
            return _currentAvailableEntityId++;
        }

        public void AddEntity(IGameEntity entity)
        {
            _entities.Add(entity);
        }

        public IGameEntity GetEntity(int entityId)
        {
            return _entities.FirstOrDefault(x => x.Id == entityId);
        }

        public void RemoveEntity(IGameEntity entity)
        {
            _entities.Remove(entity);
        }

        public void RemoveEntity(int entityId)
        {
            _entities.RemoveWhere(x => x.Id == entityId);
        }

        public void Reset()
        {
            _entities = new HashSet<IGameEntity>();
            _currentAvailableEntityId = int.MinValue;
        }
    }
}