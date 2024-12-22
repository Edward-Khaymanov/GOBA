using Cysharp.Threading.Tasks;
using GOBA.CORE;
using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

namespace GOBA
{
    public class AbilityProvider : IAbilityProvider
    {
        private IResourceProvider _resourceProvider;
        private Dictionary<string, AbilityDefinition> _nameData;

        public async UniTask Initialize(IResourceProvider resourceProvider)
        {
            _resourceProvider = resourceProvider;
            _nameData = new Dictionary<string, AbilityDefinition>();
            var locations = _resourceProvider.GetLocations<TextAsset>("AbilityData");
            foreach (var location in locations)
            {
                var asset = _resourceProvider.LoadByLocation<TextAsset>(location);
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
            return _resourceProvider.LoadByKey<GameObject>(prefabName).GetComponent<AbilityBase>();
        }
    }
}