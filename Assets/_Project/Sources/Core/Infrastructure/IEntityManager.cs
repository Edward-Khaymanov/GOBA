using System;
using System.Collections.Generic;

namespace GOBA.CORE
{
    public interface IEntityManager
    {
        public event Action<IGameEntity> EntitySpawned;

        public int GetAvailableEntityId();
        public void AddEntity(IGameEntity entity);
        public void RemoveEntity(IGameEntity entity);
        public void RemoveEntity(int entityId);
        public IGameEntity GetEntity(int entityId);
        public ICollection<IGameEntity> GetEntities(IEnumerable<int> entitiesId);
        public ICollection<IGameEntity> GetEntities();
        public IUnit GetUnit(int entityId);
        public ICollection<IUnit> GetUnits();
        public IPlayerController GetLocalPlayerController();
    }
}