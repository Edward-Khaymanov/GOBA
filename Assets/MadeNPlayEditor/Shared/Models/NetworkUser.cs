using System;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

namespace MadeNPlayShared
{
    [Serializable]
    public struct NetworkUser : INetworkSerializable
    {
        [SerializeField] private ulong _clientId;
        [SerializeField] private FixedString64Bytes _name;

        public NetworkUser(ulong clientId, FixedString64Bytes name)
        {
            _clientId = clientId;
            _name = name;
        }

        public ulong ClientId => _clientId;
        public FixedString64Bytes Name => _name;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            if (serializer.IsReader)
            {
                var reader = serializer.GetFastBufferReader();
                reader.ReadValueSafe(out _clientId);
                reader.ReadValueSafe(out _name);
            }
            else
            {
                var writer = serializer.GetFastBufferWriter();
                writer.WriteValueSafe(_clientId);
                writer.WriteValueSafe(_name);
            }
        }
    }
}