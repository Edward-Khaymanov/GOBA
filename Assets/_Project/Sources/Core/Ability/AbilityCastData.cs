using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

namespace GOBA.CORE
{
    public struct AbilityCastData : INetworkSerializable
    {
        public Vector3 CastPoint;
        public NetworkBehaviourReference[] UnitsReferences;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            if (serializer.IsReader)
            {
                var reader = serializer.GetFastBufferReader();
                reader.ReadValueSafe(out CastPoint);
                reader.ReadValueSafe(out UnitsReferences);
            }
            else
            {
                if (UnitsReferences == null)
                    UnitsReferences = new NetworkBehaviourReference[0];

                var writer = serializer.GetFastBufferWriter();

                writer.WriteValueSafe(CastPoint);
                writer.WriteValueSafe(UnitsReferences);
            }
            //if (serializer.IsReader)
            //{
            //    var reader = serializer.GetFastBufferReader();
            //    reader.ReadValueSafe(out CastPoint);
            //    serializer.SerializeValue(ref UnitsReferences);
            //}
            //else
            //{
            //    var writer = serializer.GetFastBufferWriter();

            //    writer.WriteValueSafe(CastPoint);
            //    serializer.SerializeValue(ref UnitsReferences);
            //}
        }
    }
}