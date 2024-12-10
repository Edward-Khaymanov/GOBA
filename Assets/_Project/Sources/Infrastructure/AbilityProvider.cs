using Cysharp.Threading.Tasks;
using GOBA.CORE;
using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

namespace GOBA
{
    public class AbilityProvider
    {
        private AddressablesProvider _addressablesProvider;
        private Dictionary<string, AbilityDefinition> _nameData;

        public async UniTask Initialize(AddressablesProvider addressablesProvider)
        {
            _addressablesProvider = addressablesProvider;
            _nameData = new Dictionary<string, AbilityDefinition>();
            var locations = _addressablesProvider.GetLocations<TextAsset>("AbilityData");
            foreach (var location in locations)
            {
                var asset = _addressablesProvider.LoadByLocation<TextAsset>(location);
                var abilityDefinition = JsonConvert.DeserializeObject<AbilityDefinition>(asset.text);
                _nameData.Add(abilityDefinition.Name, abilityDefinition);
            }
        }

        public AbilityDefinition GetDefinition(string name)
        {
            return _nameData[name];
        }

        public AbilityBase GetAbilityTemplate(string prefabName)
        {
            return _addressablesProvider.LoadByKey<GameObject>(prefabName).GetComponent<AbilityBase>();
        }
    }
}