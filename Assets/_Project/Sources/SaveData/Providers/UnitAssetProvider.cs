using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace GOBA
{
    public static class UnitAssetProvider
    {
        private static Dictionary<int, IResourceLocation> _unitLocations;

        public static void Initialize()
        {
            _unitLocations = new Dictionary<int, IResourceLocation>();
            var units = GetLocations<UnitAsset>("unit");
            foreach (var location in units)
            {
                var asset = LoadByLocation<UnitAsset>(location);
                _unitLocations.Add(asset.Id, location);
            }
        }

        public static List<UnitAsset> GetUnits()
        {
            var result = new List<UnitAsset>();
            var units = LoadByKey<UnitAsset>(CONSTANTS.AddresablesMarks.UNIT);

            foreach (var unit in units)
            {
                var copiedUnit = UnityEngine.Object.Instantiate(unit);
                result.Add(copiedUnit);
            }
            return result;
        }

        public static UnitAsset GetUnit(int id)
        {
            var location = _unitLocations[id];
            var asset = LoadByLocation<UnitAsset>(location);
            var copy = UnityEngine.Object.Instantiate(asset);
            return copy;
        }

        public static List<HeroAsset> GetHeroes()
        {
            var result = new List<HeroAsset>();
            var heroes = LoadByKey<HeroAsset>(CONSTANTS.AddresablesMarks.UNIT);

            foreach (var hero in heroes)
            {
                var copy = UnityEngine.Object.Instantiate(hero);
                //copy.Abilities = CopyAbilities(hero.Abilities);
                result.Add(copy);
            }

            return result;
        }

        public static HeroAsset GetHero(int id)
        {
            var location = _unitLocations[id];
            var asset = LoadByLocation<HeroAsset>(location);
            var copy = UnityEngine.Object.Instantiate(asset);
            //copy.Abilities = CopyAbilities(asset.Abilities);
            return copy;
        }

        private static IList<T> LoadByKey<T>(object key)
        {
            var locations = GetLocations<T>(key);
            return LoadByLocations<T>(locations);
        }

        private static IList<T> LoadByLocations<T>(IList<IResourceLocation> locations)
        {
            var handle = Addressables.LoadAssetsAsync<T>(locations, null);
            handle.WaitForCompletion();
            return handle.Result;
        }

        private static T LoadByLocation<T>(IResourceLocation location)
        {
            var handle = Addressables.LoadAssetAsync<T>(location);
            handle.WaitForCompletion();
            return handle.Result;
        }

        private static IList<IResourceLocation> GetLocations<T>(object key)
        {
            var catalogHandle = Addressables.LoadResourceLocationsAsync(key, typeof(T));
            catalogHandle.WaitForCompletion();
            return catalogHandle.Result;
        }

        private static List<AbilityData> CopyAbilities(List<AbilityData> abilities)
        {
            var result = new List<AbilityData>();

            foreach (var abilityData in abilities)
            {
                var newData = UnityEngine.Object.Instantiate(abilityData);
                result.Add(newData);
            }

            return result;
        }
    }
}