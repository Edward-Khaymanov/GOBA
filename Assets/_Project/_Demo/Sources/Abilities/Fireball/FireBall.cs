using Cysharp.Threading.Tasks;
using GOBA.CORE;

namespace GOBA.DEMO1
{
    public class FireBall : AbilityBase
    {
        protected override async UniTask OnSpellStart()
        {
            var owner = GetOwner();
            var castPoint = GetCastPoint();
            var castRadius = GetDefinitionData<float>("CastRadius");
            var projectileSpeed = GetDefinitionData<float>("ProjectileSpeed");
            var projectileName = GetDefinitionData<string>("ProjectileName");
            var projectileId = ProjectileManager.CreateLinearProjectile(new LinearProjectileOptions()
            {
                Ability = this,
                Source = owner,
                Direction = castPoint,
                Speed = projectileSpeed,
                ProjectileName = projectileName,
                SpawnPosition = owner.Transform.position
            });

            //projectile.SetScale(new Vector3(castRadius, castRadius, castRadius));
        }
    }
}