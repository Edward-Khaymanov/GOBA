using MapModCore;
using Unity.Netcode;
using UnityEngine;

namespace GOBA
{
    public abstract class GameEntity : NetworkBehaviour, IGameEntity
    {
        private NetworkVariable<int> _entityId = new NetworkVariable<int>();

        public int Id => _entityId.Value;
        public Transform Transform => transform;
        public NetworkBehaviour NetworkBehaviour => this;

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if (IsServer)
            {
                _entityId.Value = DIContainer.EntityManager.GetAvailableEntityId();
            }
            DIContainer.EntityManager.AddEntity(this);
        }

    }
}