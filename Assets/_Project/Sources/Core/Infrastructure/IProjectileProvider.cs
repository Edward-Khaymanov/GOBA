using UnityEngine;

namespace GOBA.CORE
{
    public interface IProjectileProvider
    {
        public GameObject GetProjectile(string projectileName);
        public AbilityProjectile GetProjectileWrapper();
    }
}