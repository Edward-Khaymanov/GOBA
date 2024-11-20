using MapModCore;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;

namespace GOBA
{
    public class EntityManager : IEntityManager
    {
        private HashSet<IGameEntity> _entities = new HashSet<IGameEntity>();
        private int _currentAvailableEntityId = int.MinValue;

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

        public IUnit GetUnit(int entityId)
        {
            return _entities.FirstOrDefault(x => x.Id == entityId && x is IUnit) as IUnit;
        }

        public void RemoveEntity(IGameEntity entity)
        {
            _entities.Remove(entity);
        }

        public void Reset()
        {
            _entities = new HashSet<IGameEntity>();
            _currentAvailableEntityId = int.MinValue;
        }
    }
}