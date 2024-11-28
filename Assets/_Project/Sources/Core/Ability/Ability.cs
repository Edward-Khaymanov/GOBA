using System;
using Unity.Netcode;

namespace GOBA.CORE
{
    public abstract class Ability : INetworkSerializable, IEquatable<Ability>
    {
        private int _ownerEntityId;
        //private Dictionary<string, string> _keyValues = new Dictionary<string, string>();

        public abstract int Id { get; }

        public virtual void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref _ownerEntityId);

            //if (serializer.IsReader)
            //{
            //    _keyValues.Clear();
            //    var reader = serializer.GetFastBufferReader();
            //    reader.ReadValue(out int kvcount);

            //    for (int i = 0; i < kvcount; i++)
            //    {
            //        reader.ReadValue(out string key);
            //        reader.ReadValue(out string value);
            //        _keyValues.Add(key, value);
            //    }
            //}
            //else if (serializer.IsWriter)
            //{
            //    var writer = serializer.GetFastBufferWriter();
            //    writer.WriteValue(_keyValues.Count);

            //    foreach (var keyValue in _keyValues)
            //    {
            //        writer.WriteValue(keyValue.Key);
            //        writer.WriteValue(keyValue.Value);
            //    }
            //}
        }
        //public object GetData(string key)
        //{
        //    return this.getm
        //}
        //public void SetData(string key, object value)
        //{

        //}

        public bool Equals(Ability other)
        {
            return Id == other.Id;
        }

        public abstract void Use(AbilityCastData castData);
        public abstract AbilityBehaviour GetBehaviour();
        public abstract AbilityUnitTargetType GetTargetType();
        public abstract AbilityUnitTargetTeam GetTargetTeam();



        public IUnit GetOwner()
        {
            return DIContainer.EntityManager.GetEntity(_ownerEntityId) as IUnit;
        }
        public void SetOwner(int ownerEntityId)
        {
            _ownerEntityId = ownerEntityId;
        }
        public virtual float GetCooldownTimeRemaining() => 0f;
        public virtual float GetCooldown(int level) => 0f;
        public virtual void StartCooldown(float cooldown) { }
        public virtual void EndCooldown() { }
    }
}