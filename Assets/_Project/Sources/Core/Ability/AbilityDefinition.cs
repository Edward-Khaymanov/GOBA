using Newtonsoft.Json.Linq;
using System;

namespace GOBA.CORE
{
    [Serializable]
    public class AbilityDefinition
    {
        public int Id;
        public string ScriptFileName;
        public string IconTextureName;
        public string NameKey;
        public string DescriptionKey;
        //public JObject Art;
        //public JObject Data;
        //public JObject SpecialValues;
        public JObject Data;
    }
}