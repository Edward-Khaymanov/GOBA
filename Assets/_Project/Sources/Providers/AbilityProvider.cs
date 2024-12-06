using Cysharp.Threading.Tasks;
using GOBA.CORE;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace GOBA
{
    public enum MyEnum
    {
        
    }

    public class AbilityProvider
    {
        private Dictionary<int, JObject> _idData;

        public async UniTask Initialize()
        {
            _idData = new Dictionary<int, JObject>();
            var locations = AddressablesHelper.GetLocations<TextAsset>("AbilityData");
            foreach (var location in locations)
            {
                var asset = AddressablesHelper.LoadByLocation<TextAsset>(location);
                var abilityData = JsonConvert.DeserializeObject<AbilityDefinition>(asset.text);
                //_idData.Add(abilityData.Id, abilityData);
            }
        }

        public JObject Get(int id)
        {
            return _idData[id];
        }

    }
}