using GOBA.CORE;
using System.Collections.Generic;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace GOBA
{
    public class UnitAssetProvider : IUnitAssetProvider
    {
        private IResourceProvider _resourceProvider;
        private Dictionary<int, IResourceLocation> _unitLocations;
        private Dictionary<int, IResourceLocation> _heroLocations;

        public void Initialize(IResourceProvider resourceProvider)
        {
            _resourceProvider = resourceProvider;
            _unitLocations = new Dictionary<int, IResourceLocation>();
            _heroLocations = new Dictionary<int, IResourceLocation>();

            var units = _resourceProvider.GetLocations<UnitAsset>(CONSTANTS.AddresablesMarks.UNIT);
            foreach (var location in units)
            {
                var asset = _resourceProvider.LoadByLocation<UnitAsset>(location);
                _unitLocations.Add(asset.Id, location);
                if (asset is HeroAsset)
                {
                    _heroLocations.Add(asset.Id, location);
                }
            }
        }

        public List<UnitAsset> GetUnits()
        {
            var result = new List<UnitAsset>();

            foreach (var location in _unitLocations.Values)
            {
                var asset = _resourceProvider.LoadByLocation<UnitAsset>(location);
                var copiedUnit = UnityEngine.Object.Instantiate(asset);
                result.Add(copiedUnit);
            }
            return result;
        }

        public UnitAsset GetUnit(int id)
        {
            var location = _unitLocations[id];
            var asset = _resourceProvider.LoadByLocation<UnitAsset>(location);
            var copy = UnityEngine.Object.Instantiate(asset);
            return copy;
        }

        public List<HeroAsset> GetHeroes()
        {
            var result = new List<HeroAsset>();

            foreach (var location in _heroLocations.Values)
            {
                var asset = _resourceProvider.LoadByLocation<HeroAsset>(location);
                var copy = UnityEngine.Object.Instantiate(asset);
                result.Add(copy);
            }

            return result;
        }

        public HeroAsset GetHero(int id)
        {
            var location = _unitLocations[id];
            var asset = _resourceProvider.LoadByLocation<HeroAsset>(location);
            var copy = UnityEngine.Object.Instantiate(asset);
            return copy;
        }
    }
}