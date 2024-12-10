namespace GOBA.CORE
{
    public interface IProjectileProvider
    {
        public AbilityProjectile GetProjectileTemplate(string projectileName);
    }
}