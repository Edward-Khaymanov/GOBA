using Cysharp.Threading.Tasks;
using GOBA.CORE;
using Unity.Netcode;

namespace GOBA.DEMO1
{
    public class FireBall : AbilityBase
    {
        protected override void OnSync<T>(BufferSerializer<T> serializer)
        {
        }

        protected override async UniTask OnSpellStart(AbilityCastData castData)
        {
            var owner = GetOwner();
            var castRadius = GetDefinitionData<float>("CastRadius");
            var projectileSpeed = GetDefinitionData<float>("ProjectileSpeed");
            var projectileName = GetDefinitionData<string>("ProjectileName");
            var projectileId = ProjectileManager.CreateLinearProjectile(new LinearProjectileOptions()
            {
                Ability = this,
                Source = owner,
                Direction = castData.CastPoint,
                Speed = projectileSpeed,
                ProjectileName = projectileName,
                SpawnPosition = owner.Transform.position
            });
            
            //projectile.SetScale(new Vector3(castRadius, castRadius, castRadius));
        }
    }
}