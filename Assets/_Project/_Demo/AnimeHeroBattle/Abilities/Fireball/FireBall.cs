using Cysharp.Threading.Tasks;
using GOBA.CORE;
using System.Threading;
using Unity.Netcode;
using UnityEngine;

namespace AnimeHeroBattle
{
    public class FireBall : Ability
    {
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private int _projectileId;
        private float _currentCooldown;

        public override void Initialize(AbilityDefinition definition)
        {
            base.Initialize(definition);
            _cancellationTokenSource = new CancellationTokenSource();
            //_data = definition.Data;
        }

        public FireBall(FireBallData data, int projectileId)
        {
            _cancellationTokenSource = new CancellationTokenSource();
            //_data = data;
            _projectileId = projectileId;
        }

        protected override void OnSync<T>(BufferSerializer<T> serializer)
        {
            //serializer.SerializeValue(ref _data);
            serializer.SerializeValue(ref _currentCooldown);
        }

        public override void CastAbility(AbilityCastData castData)
        {
            _cancellationTokenSource = new CancellationTokenSource();
            Use(castData, _cancellationTokenSource.Token).Forget();
        }

        public override void StartCooldown(float cooldown)
        {
            StartCooldownAsync(cooldown).Forget();
        }

        public override float GetCooldownTimeRemaining()
        {
            return _currentCooldown;
        }



        private async UniTaskVoid Use(AbilityCastData castData, CancellationToken cancellationToken)
        {
            var owner = GetOwner();
            var castRadius = GetDefinitionData<float>("CastRadius");
            var projectileSpeed = GetDefinitionData<float>("ProjectileSpeed");
            var cooldown = GetDefinitionData<float>("Cooldown");
            var projectile = GameObject.Instantiate(DIContainer.ProjectileProvider.GetProjectile(_projectileId), owner.Transform.position, Quaternion.identity);
            projectile.NetworkObject.SpawnWithOwnership(owner.NetworkBehaviour.OwnerClientId);
            projectile.SetScale(new Vector3(castRadius, castRadius, castRadius));
            projectile.SetSpeed(projectileSpeed);
            projectile.MoveTo(castData.CastPoint).Forget();
            StartCooldown(cooldown);

        }

        private async UniTaskVoid StartCooldownAsync(float cooldown)
        {
            var newCooldown = cooldown;
            _currentCooldown = newCooldown;

            while (_currentCooldown > 0f)
            {
                await UniTask.NextFrame();
                newCooldown -= Time.deltaTime;
                if (newCooldown < 0f)
                    newCooldown = 0f;

                _currentCooldown = newCooldown;
            }
        }
    }
}