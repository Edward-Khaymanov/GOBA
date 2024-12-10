using Newtonsoft.Json.Linq;
using System;

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
    public class AbilityDefinition
    {
        public int Id;
        public string PrefabName;
        public string IconTextureName;
        public string Name;
        public string DescriptionKey;
        //public JObject Art;
        //public JObject Data;
        //public JObject SpecialValues;
        public JObject Data;
    }
}