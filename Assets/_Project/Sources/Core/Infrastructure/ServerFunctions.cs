using Cysharp.Threading.Tasks;
using GOBA.CORE;
using System.Collections.Generic;
using System.Linq;
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

        public static void ApplyDamage(
            IUnit target,
            IUnit attacker,
            float damage,
            DamageType damageType,
            AbilityBase ability = default)
        {
            var resistMultiplier = 0f;
            damage = Mathf.Max(damage, 0);
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

        public static IEnumerable<IUnit> FindUnits(
            int teamId,
            UnitTargetTeam teamFilter = UnitTargetTeam.ALL,
            UnitTargetType typeFilter = UnitTargetType.ALL)
        {
            var allUnits = _entityManager.GetUnits();
            var filtredUnits = FilterByTeam(allUnits, teamId, teamFilter);
            filtredUnits = FilterByType(filtredUnits, typeFilter);
            filtredUnits = FilterByFlags(filtredUnits);

            return filtredUnits;
        }


        public static void Heal(IUnit target, float amount)
        {
            amount = Mathf.Max(amount, 0);
            var newHealth = target.GetHealth() + amount;
            target.SetHealth(newHealth);
        }

        private static IEnumerable<IUnit> FilterByTeam(IEnumerable<IUnit> unitList, int teamId, UnitTargetTeam teamFilter)
        {
            var result = new List<IUnit>();
            if (teamFilter.HasFlag(UnitTargetTeam.ENEMY))
            {
                result.AddRange(unitList.Where(x => x.GetTeam() != teamId));//later check by team manager
            }

            if (teamFilter.HasFlag(UnitTargetTeam.FRIENDLY))
            {
                result.AddRange(unitList.Where(x => x.GetTeam() == teamId));//later check by team manager
            }

            return result;
        }

        private static IEnumerable<IUnit> FilterByType(IEnumerable<IUnit> unitList, UnitTargetType typeFilter)
        {
            var result = new List<IUnit>();

            if (typeFilter.HasFlag(UnitTargetType.ALL))
            {
                return unitList;
            }

            if (typeFilter.HasFlag(UnitTargetType.BASIC))
            {
                result.AddRange(unitList.Where(x => x is IUnit));
            }

            if (typeFilter.HasFlag(UnitTargetType.HERO))
            {
                result.AddRange(unitList.Where(x => x is IHero));
            }

            return result;
        }

        private static IEnumerable<IUnit> FilterByFlags(IEnumerable<IUnit> unitList)
        {
            return unitList.Where(x => x.IsDead() == false);
        }
    }
}