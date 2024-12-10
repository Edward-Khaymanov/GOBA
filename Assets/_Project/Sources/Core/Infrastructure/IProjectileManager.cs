using UnityEngine;

namespace GOBA.CORE
{
    public class BaseProjectileOptions
    {
        public string ProjectileName;
        public AbilityBase Ability;
        public IUnit Source;
        public Vector3 SpawnPosition;
    }

    public class LinearProjectileOptions : BaseProjectileOptions
    {
        public float Speed;
        public Vector3 Direction;
    }

    public interface IProjectileManager
    {
        public int CreateLinearProjectile(LinearProjectileOptions options);
        public void DestoryProjectile(int projectileId);
        public void DestoryProjectile(AbilityProjectile abilityProjectile);
    }
}