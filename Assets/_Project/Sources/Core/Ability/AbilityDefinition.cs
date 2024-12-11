using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using Unity.Netcode;

namespace GOBA.CORE
{
    /*
    ЗАНЯТЫЕ ПОЛЯ В DATA
    AbilityBehaviour
    AbilityUnitTargetType
    AbilityUnitTargetTeam
    Cost                                        float[]
    MaxLevel                                    int
    Cooldown                                    float[]
    CastTime                                    float
     
    */

    [Serializable]
    public class AbilityDefinition : INetworkSerializable, IEquatable<AbilityDefinition>
    {
        public int Id;
        public string Name = string.Empty;
        public string PrefabName = string.Empty;
        public string IconTextureName = string.Empty;
        public string DescriptionKey = string.Empty;
        //public JObject Art;
        //public JObject Data;
        //public JObject SpecialValues;
        public JObject Data = new JObject();

        public bool Equals(AbilityDefinition other)
        {
            return this.Id == other.Id;
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref Id);
            serializer.SerializeValue(ref Name);
            serializer.SerializeValue(ref IconTextureName);
            serializer.SerializeValue(ref DescriptionKey);
            if (serializer.IsWriter)
            {
                var writer = serializer.GetFastBufferWriter();
                var json = JsonConvert.SerializeObject(Data);
                writer.WriteValueSafe(json);
            }
            else
            {
                var reader = serializer.GetFastBufferReader();
                reader.ReadValueSafe(out string json);
                Data = JObject.Parse(json);
            }
        }
    }
}