using System.Collections.Generic;

namespace GOBA.CORE
{
    public interface IUnitAssetProvider
    {
        public List<UnitAsset> GetUnits();
        public UnitAsset GetUnit(int id);
        public List<HeroAsset> GetHeroes();
        public HeroAsset GetHero(int id);
    }
}