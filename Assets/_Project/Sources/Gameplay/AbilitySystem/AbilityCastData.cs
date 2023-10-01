using Unity.Netcode;
using UnityEngine;

namespace GOBA
{
    public struct AbilityCastData : INetworkSerializable
    {
        public NetworkBehaviourReference CasterReference;
        public Vector3 CastPoint;
        public NetworkBehaviourReference[] UnitsReferences;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            if (serializer.IsReader)
            {
                var reader = serializer.GetFastBufferReader();

                serializer.SerializeValue(ref CasterReference);
                reader.ReadValueSafe(out CastPoint);
                serializer.SerializeValue(ref UnitsReferences);
            }
            else
            {
                var writer = serializer.GetFastBufferWriter();

                serializer.SerializeValue(ref CasterReference);
                writer.WriteValueSafe(CastPoint);
                serializer.SerializeValue(ref UnitsReferences);
            }
        }
    }
}