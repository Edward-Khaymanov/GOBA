using GOBA.CORE;
using UnityEngine;

namespace GOBA
{
    public static class DevCheats
    {
        private static IEntityManager _entityManager;
        private static IParticleManager _particleManager;
        private static IProjectileManager _projectileManager;
        private static IProjectileProvider _projectileProvider;
        private static AbilityProvider _abilityProvider;

        public static void Init(
            IEntityManager entityManager, 
            IParticleManager particleManager, 
            IProjectileManager projectileManager, 
            IProjectileProvider projectileProvider,
            AbilityProvider abilityProvider)
        {
            _entityManager = entityManager;
            _particleManager = particleManager;
            _projectileManager = projectileManager;
            _projectileProvider = projectileProvider;
            _abilityProvider = abilityProvider;
        }

        public static AbilityBase AddAbilityToUnit(string abilityName, IUnit unit)
        {
            var abilityDef = _abilityProvider.GetDefinition(abilityName);
            var abilityTemplate = _abilityProvider.GetAbilityTemplate(abilityDef.PrefabName);
            var ability = GameObject.Instantiate(abilityTemplate, Vector3.zero, Quaternion.identity);
            ability.NetworkObject.Spawn();
            ability.SetDependencies(_projectileManager, _particleManager);
            ability.Initialize(abilityDef);
            ability.SetOwner(unit.EntityId);
            unit.AddAbility(ability);
            return ability;
        }

        public static void SetAbilityLevel(int abilityEntityId, int level)
        {
            var ability = _entityManager.GetEntity(abilityEntityId) as AbilityBase;
            ability.SetLevel(level);
        }
    }
}