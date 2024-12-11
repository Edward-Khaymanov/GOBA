using Cysharp.Threading.Tasks;
using GOBA.CORE;
using System.Collections.Generic;
using UnityEngine;

namespace GOBA
{
    public class ProjectileProvider : IProjectileProvider
    {
        private AddressablesProvider _addressablesProvider;
        private Dictionary<string, GameObject> _nameTemplate;
        private AbilityProjectile _abilityProjectileWrapperTemplate;

        public async UniTask Initialize(AddressablesProvider addressablesProvider)
        {
            _addressablesProvider = addressablesProvider;
            _nameTemplate = new Dictionary<string, GameObject>();
            _abilityProjectileWrapperTemplate = _addressablesProvider.LoadByKey<GameObject>("AbilityProjectileWrapper").GetComponent<AbilityProjectile>();
            var locations = _addressablesProvider.GetLocations<GameObject>("AbilityProjectile");

            foreach (var location in locations)
            {
                var projectile = _addressablesProvider.LoadByLocation<GameObject>(location);
                _nameTemplate.Add(location.PrimaryKey, projectile);
            }
        }

        public GameObject GetProjectile(string projectileName)
        {
            return GameObject.Instantiate(_nameTemplate[projectileName]);
        }

        public AbilityProjectile GetProjectileWrapper()
        {
            return GameObject.Instantiate(_abilityProjectileWrapperTemplate);
        }
    }
}