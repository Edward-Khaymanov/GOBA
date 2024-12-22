using Cysharp.Threading.Tasks;
using GOBA.CORE;
using System.Collections.Generic;
using UnityEngine;

namespace GOBA
{
    public class ProjectileProvider : IProjectileProvider
    {
        private IResourceProvider _resourceProvider;
        private Dictionary<string, GameObject> _nameTemplate;
        private AbilityProjectile _abilityProjectileWrapperTemplate;

        public async UniTask Initialize(IResourceProvider resourceProvider)
        {
            _resourceProvider = resourceProvider;
            _nameTemplate = new Dictionary<string, GameObject>();
            _abilityProjectileWrapperTemplate = _resourceProvider.LoadByKey<GameObject>("AbilityProjectileWrapper").GetComponent<AbilityProjectile>();
            var locations = _resourceProvider.GetLocations<GameObject>("AbilityProjectile");

            foreach (var location in locations)
            {
                var projectile = _resourceProvider.LoadByLocation<GameObject>(location);
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