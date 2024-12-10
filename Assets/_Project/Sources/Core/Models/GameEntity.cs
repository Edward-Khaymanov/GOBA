using GOBA.CORE;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

namespace GOBA
{
    public abstract class GameEntity : NetworkBehaviour, IGameEntity
    {
        private NetworkVariable<int> _entityId = new NetworkVariable<int>();

        public int EntityId => _entityId.Value;
        public Transform Transform => transform;
        public NetworkBehaviour NetworkBehaviour => this;

        public sealed override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if (IsServer)
            {
                _entityId.Value = DIContainer.EntityManager.GetAvailableEntityId();
            }
            DIContainer.EntityManager.AddEntity(this);
        }

        public sealed override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();
            DIContainer.EntityManager.RemoveEntity(this);
        }
    }
}