using GOBA.CORE;
using System.Collections.Generic;
using System.Linq;

namespace GOBA
{
    public class EntityManager : IEntityManager
    {
        private readonly Dictionary<int, IGameEntity> _entities;
        private IPlayerController _localPlayerController;
        private int _currentAvailableEntityId;

        public event System.Action<IGameEntity> EntitySpawned;

        public EntityManager()
        {
            _entities = new Dictionary<int, IGameEntity>();
            _currentAvailableEntityId = int.MinValue;
        }

        public int GetAvailableEntityId()
        {
            return _currentAvailableEntityId++;
        }

        public void AddEntity(IGameEntity entity)
        {
            if (_entities.TryAdd(entity.EntityId, entity))
            {
                if (entity is IPlayerController controller && controller.NetworkBehaviour.IsOwner)
                {
                    _localPlayerController = controller;
                }

                EntitySpawned?.Invoke(entity);
            }
        }

        public IGameEntity GetEntity(int entityId)
        {
            return _entities.GetValueOrDefault(entityId);
        }

        public void RemoveEntity(IGameEntity entity)
        {
            _entities.Remove(entity.EntityId);
        }

        public void RemoveEntity(int entityId)
        {
            _entities.Remove(entityId);
        }

        public ICollection<IGameEntity> GetEntities(IEnumerable<int> entitiesId)
        {
            return _entities.Where(x => entitiesId.Contains(x.Key)).Select(x => x.Value).ToArray();
        }

        public ICollection<IGameEntity> GetEntities()
        {
            return _entities.Values;
        }

        public IUnit GetUnit(int entityId)
        {
            var result = default(IUnit);
            if (_entities.TryGetValue(entityId, out var entity) && entity is IUnit unit)
            {
                result = unit;
            }
            return result;
        }

        public ICollection<IUnit> GetUnits()
        {
            return _entities.Values.Where(x => x is IUnit unit).Cast<IUnit>().ToArray();
        }

        public IPlayerController GetLocalPlayerController()
        {
            return _localPlayerController;
        }
    }
}