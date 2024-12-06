using GOBA.CORE;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Unity.Netcode;
using UnityEngine;

namespace GOBA
{
    public class NetworkAbilityList : NetworkVariableBase
    {
        private readonly List<Ability> _value = new List<Ability>(8);

        public IList<Ability> Value => _value;

        public NetworkAbilityList(IEnumerable<Ability> values = default,
            NetworkVariableReadPermission readPerm = DefaultReadPerm,
            NetworkVariableWritePermission writePerm = DefaultWritePerm)
            : base(readPerm, writePerm)
        {
            if (values == default)
                return;

            foreach (var ability in values)
            {
                _value.Add(ability);
            }
        }

        public void Add(Ability ability)
        {
            if (!CanClientWrite(this.GetBehaviour().NetworkManager.LocalClientId))
            {
                //LogWritePermissionError();
                return;
            }

            _value.Add(ability);

            //var listEvent = new NetworkListEvent<Ability>()
            //{
            //    Type = NetworkListEvent<Ability>.EventType.Add,
            //    Value = ability,
            //    Index = _value.Count - 1
            //};
            SetDirty(true);
        }

        public override void WriteField(FastBufferWriter writer)
        {
            Debug.Log("WriteField");
            writer.WriteValueSafe(_value.Count);
            foreach (var value in _value)
            {
                var type = value.GetType();
                var json = JsonConvert.SerializeObject(type);
                var bytes = Encoding.UTF8.GetBytes(json);

                BytePacker.WriteValuePacked(writer, bytes.Length);
                writer.WriteBytesSafe(bytes);
                writer.WriteNetworkSerializable(value);
            }
        }

        public override void ReadField(FastBufferReader reader)
        {
            Debug.Log("ReadField");
            _value.Clear();
            reader.ReadValueSafe(out int count);
            for (int i = 0; i < count; i++)
            {
                ByteUnpacker.ReadValuePacked(reader, out int length);
                var bytes = new byte[length];
                reader.ReadBytesSafe(ref bytes, length);
                var json = Encoding.UTF8.GetString(bytes);
                var type = JsonConvert.DeserializeObject<Type>(json);
                var ability = Activator.CreateInstance(type) as Ability;
                reader.ReadNetworkSerializableInPlace(ref ability);
                _value.Add(ability);
            }
        }

        public override void WriteDelta(FastBufferWriter writer)
        {
            WriteField(writer);
        }

        public override void ReadDelta(FastBufferReader reader, bool keepDirtyDelta)
        {
            ReadField(reader);
        }
    }
}