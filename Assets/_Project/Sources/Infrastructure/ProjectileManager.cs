using GOBA.CORE;
using System;
using UnityEngine;

namespace GOBA
{
    public class ProjectileManager : IProjectileManager
    {
        private readonly IProjectileProvider _projectileProvider;

        public ProjectileManager(IProjectileProvider projectileProvider)
        {
            _projectileProvider = projectileProvider;
        }

        public int CreateLinearProjectile(LinearProjectileOptions options)
        {
            var projectileTemplate = _projectileProvider.GetProjectileTemplate(options.ProjectileName);
            var projectile = GameObject.Instantiate(projectileTemplate, options.SpawnPosition, Quaternion.identity);
            projectile.NetworkObject.Spawn();
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