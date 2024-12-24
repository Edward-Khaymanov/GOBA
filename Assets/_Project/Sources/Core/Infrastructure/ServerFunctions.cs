using Cysharp.Threading.Tasks;
using GOBA.CORE;
using Unity.Netcode;
using UnityEngine;

namespace GOBA
{
    public static class ServerFunctions
    {
        public const string HERO_TEMPLATE_KEY = "HeroTemplate";

        private static IEntityManager _entityManager;
        private static IParticleManager _particleManager;
        private static IProjectileManager _projectileManager;
        private static IProjectileProvider _projectileProvider;
        private static IAbilityProvider _abilityProvider;
        private static IResourceProvider _resourceProvider;
        private static IUnitAssetProvider _unitAssetProvider;

        public static void Init(
            IEntityManager entityManager, 
            IParticleManager particleManager, 
            IProjectileManager projectileManager, 
            IProjectileProvider projectileProvider,
            IAbilityProvider abilityProvider,
            IResourceProvider resourceProvider,
            IUnitAssetProvider unitAssetProvider)
        {
            _entityManager = entityManager;
            _particleManager = particleManager;
            _projectileManager = projectileManager;
            _projectileProvider = projectileProvider;
            _abilityProvider = abilityProvider;
            _resourceProvider = resourceProvider;
            _unitAssetProvider = unitAssetProvider;
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

        public static async UniTask<IHero> SpawnHero(int heroId, int teamId, Vector3 position)
        {
            var heroAsset = _unitAssetProvider.GetHero(heroId);
            var heroTemplate = _resourceProvider.LoadByKey<GameObject>(HERO_TEMPLATE_KEY);
            var heroModelTemplate = _resourceProvider.LoadByKey<UnitModel>(heroAsset.ModelKey);
            var hero = GameObject.Instantiate(heroTemplate, position, Quaternion.identity).GetComponent<IHero>();
            var heroModel = GameObject.Instantiate(heroModelTemplate, Vector3.zero, Quaternion.identity);

            hero.SetTeam(teamId);
            hero.SetMaxHealth(2000);
            hero.SetHealth(2000);
            hero.SetMaxMana(1000);
            hero.SetMana(1000);
            hero.NetworkBehaviour.NetworkObject.Spawn();
            heroModel.GetComponent<NetworkObject>().Spawn();
            heroModel.GetComponent<NetworkObject>().TrySetParent(hero.NetworkBehaviour.NetworkObject, false);
            hero.Initialize();
            return hero;
        }

        public static void ApplyDamage(IUnit target, IUnit attacker, float damage, DamageType damageType, AbilityBase ability = default)
        {
            var resistMultiplier = 0f;
            damage = Mathf.Abs(damage);
            damage = damage * (1 - resistMultiplier);
            var newHealth = target.GetHealth() - damage;
            target.SetHealth(newHealth);
            //log damage
            if (target.GetHealth() == 0)
            {
                target.Kill();
                //log kill 
            }
        }
    }
}