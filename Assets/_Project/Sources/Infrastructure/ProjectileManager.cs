using GOBA.CORE;
using Unity.Netcode;

namespace GOBA
{
    public class ProjectileManager : IProjectileManager
    {
        private readonly IProjectileProvider _projectileProvider;
        //private readonly IParticleManager _particleManager;
        public ProjectileManager(IProjectileProvider projectileProvider)
        {
            _projectileProvider = projectileProvider;
        }

        public int CreateLinearProjectile(LinearProjectileOptions options)
        {
            var projectile = _projectileProvider.GetProjectileWrapper();
            var projectileEffect = _projectileProvider.GetProjectile(options.ProjectileName);
            projectile.Transform.position = options.SpawnPosition;
            projectile.NetworkObject.Spawn();
            projectileEffect.GetComponent<NetworkObject>().Spawn();
            projectileEffect.GetComponent<NetworkObject>().TrySetParent(projectile.Transform, false);
            projectile.Init(this);
            projectile.SetAbility(options.Ability);
            projectile.SetSpeed(options.Speed);
            projectile.MoveToPoint(options.Direction);/////////временно
            return projectile.EntityId;
        }

        public void DestoryProjectile(int projectileId)
        {
            var abilityProjectile = DIContainer.EntityManager.GetEntity(projectileId);
            abilityProjectile.NetworkBehaviour.NetworkObject.Despawn();
        }

        public void DestoryProjectile(AbilityProjectile abilityProjectile)
        {
            abilityProjectile.NetworkBehaviour.NetworkObject.Despawn();
        }
    }
}