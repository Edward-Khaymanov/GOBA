using Unity.Netcode;
using UnityEngine;

namespace GOBA.CORE
{
    public struct AbilityCastData : INetworkSerializeByMemcpy
    {
        public Vector3 CastPoint;
        public int TargetEntityId;
    }
}