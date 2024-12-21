using Cysharp.Threading.Tasks;
using GOBA.CORE;
using Unity.Netcode;
using UnityEngine;

namespace GOBA
{
    public static class ServerFunctions
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

            ability.SetDependencies(_projectileManager, _particleManager, _entityManager);
            ability.Initialize(abilityDef);
            ability.SetOwner(unit.EntityId);
            ability.NetworkObject.Spawn();
            unit.AddAbility(ability);

            return ability;
        }

        public static void SetAbilityLevel(int abilityEntityId, int level)
        {
            var ability = _entityManager.GetEntity(abilityEntityId) as AbilityBase;
            ability.SetLevel(level);
        }

        public static async UniTask<Hero> SpawnHero(int heroId, int teamId, Vector3 position)
        {
            var heroTemplate = new AddressablesProvider().LoadByKey<GameObject>(CONSTANTS.HERO_TEMPLATE_KEY).GetComponent<Hero>();
            var heroAsset = UnitAssetProvider.GetHero(heroId);
            var hero = GameObject.Instantiate(heroTemplate, position, Quaternion.identity);
            var heroModel = GameObject.Instantiate(heroAsset.Model, Vector3.zero, Quaternion.identity);
            hero.SetTeam(teamId);
            hero.NetworkObject.Spawn();
            heroModel.GetComponent<NetworkObject>().Spawn();
            heroModel.GetComponent<NetworkObject>().TrySetParent(hero.NetworkBehaviour.NetworkObject, false);
            hero.Initialize();
            return hero;
        }
    }
}