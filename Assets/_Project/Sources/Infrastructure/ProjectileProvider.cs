using Cysharp.Threading.Tasks;
using GOBA.CORE;
using System.Collections.Generic;
using UnityEngine;

namespace GOBA
{
    public class ProjectileProvider : IProjectileProvider
    {
        private AddressablesProvider _addressablesProvider;
        private Dictionary<string, AbilityProjectile> _nameTemplate;

        public async UniTask Initialize(AddressablesProvider addressablesProvider)
        {
            _addressablesProvider = addressablesProvider;
            _nameTemplate = new Dictionary<string, AbilityProjectile>();
            var locations = _addressablesProvider.GetLocations<GameObject>("AbilityProjectile");

            foreach (var location in locations)
            {
                var projectile = _addressablesProvider.LoadByLocation<GameObject>(location).GetComponent<AbilityProjectile>();
                _nameTemplate.Add(location.PrimaryKey, projectile);
            }
        }

        public AbilityProjectile GetProjectileTemplate(string projectileName)
        {
            return _nameTemplate[projectileName];
        }
    }
}