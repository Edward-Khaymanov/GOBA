using System;
using System.Security.Cryptography;
using Unity.Netcode;

namespace GOBA.CORE
{
    public interface IAbilityBody
    {
        public void OnSync<T>(BufferSerializer<T> serializer) where T : IReaderWriter;
    }

    public class FireD : IAbilityBody
    {
        private int id;
        private int zxc;

        public void OnSync<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref id);
            serializer.SerializeValue(ref zxc);
        }
    }

    public abstract class Ability : INetworkSerializable, IEquatable<Ability>
    {
        private int _id;
        private AbilityDefinition _abilityDefinition;
        private int _ownerEntityId;
        private IAbilityBody _body;
        //private Dictionary<string, string> _keyValues = new Dictionary<string, string>();

        public int Id => _id;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref _id);
            serializer.SerializeValue(ref _ownerEntityId);
            OnSync(serializer);
            _body.OnSync(serializer);
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
            return _id == other._id;
        }

        public abstract void CastAbility(AbilityCastData castData);

        public AbilityBehaviour GetBehaviour()
        {
            return GetDefinitionData<AbilityBehaviour>("AbilityBehaviour");
        }
        public AbilityUnitTargetType GetTargetType()
        {
            return GetDefinitionData<AbilityUnitTargetType>("AbilityUnitTargetType");

        }
        public AbilityUnitTargetTeam GetTargetTeam()
        {
            return GetDefinitionData<AbilityUnitTargetTeam>("AbilityUnitTargetTeam");
        }


        public virtual void Initialize(AbilityDefinition definition)
        {
            _id = definition.Id;
            _abilityDefinition = definition;
        }
        public IUnit GetOwner()
        {
            return DIContainer.EntityManager.GetEntity(_ownerEntityId) as IUnit;
        }
        public void SetOwner(int ownerEntityId)
        {
            _ownerEntityId = ownerEntityId;
        }
        public virtual float GetCooldownTimeRemaining() => throw new NotImplementedException();
        public virtual float GetCooldown(int level) => throw new NotImplementedException();
        public virtual void StartCooldown(float cooldown) => throw new NotImplementedException();
        public virtual void EndCooldown() => throw new NotImplementedException();

        protected abstract void OnSync<T>(BufferSerializer<T> serializer) where T : IReaderWriter;

        protected T GetDefinitionData<T>(string key)
        {
            return _abilityDefinition.Data.Value<T>(key);
        }
    }
}