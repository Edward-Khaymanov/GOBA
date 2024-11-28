using Unity.Netcode;
using UnityEngine;

namespace GOBA.CORE
{
    public interface IGameEntity
    {
        public int Id { get; }
        public Transform Transform { get; }
        public NetworkBehaviour NetworkBehaviour { get; }
    }
}