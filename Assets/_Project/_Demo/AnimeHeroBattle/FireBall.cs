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
        private FireBallData _data;
        private int _projectileId;
        private float _currentCooldown;

        public FireBall()
        {
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public FireBall(FireBallData data, int projectileId)
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _data = data;
            _projectileId = projectileId;
        }

        public override int Id => _data.Id;

        public override void NetworkSerialize<T>(BufferSerializer<T> serializer)
        {
            base.NetworkSerialize(serializer);
            serializer.SerializeValue(ref _data);
            serializer.SerializeValue(ref _currentCooldown);
            serializer.SerializeValue(ref _projectileId);
        }

        public override void Use(AbilityCastData castData)
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
            var projectile = GameObject.Instantiate(DIContainer.ProjectileProvider.GetProjectile(_projectileId), owner.Transform.position, Quaternion.identity);
            projectile.NetworkObject.SpawnWithOwnership(owner.NetworkBehaviour.OwnerClientId);
            projectile.SetScale(new Vector3(_data.CastRadius * 2, _data.CastRadius * 2, _data.CastRadius * 2));
            projectile.SetSpeed(_data.ProjectileSpeed);
            projectile.MoveTo(castData.CastPoint).Forget();
            StartCooldown(_data.Cooldown);

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

        public override AbilityBehaviour GetBehaviour()
        {
            return _data.Behaviour;
        }

        public override AbilityUnitTargetType GetTargetType()
        {
            return _data.UnitTargetType;
        }

        public override AbilityUnitTargetTeam GetTargetTeam()
        {
            return _data.UnitTargetTeam;
        }
    }
}