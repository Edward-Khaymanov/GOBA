using Cysharp.Threading.Tasks;
using System;
using System.Drawing;
using System.Threading;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;

namespace GOBA.CORE
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    public class AbilityProjectile : GameEntity
    {
        private Rigidbody _rigidbody;
        private float _speed;
        private AbilityBase _ability;
        private IUnit _source;
        private IProjectileManager _projectileManager;

        private const float DEFAULT_LEVEL_Y = 40f;
        private const float RAYCAST_Y = 100f;
        private const float PROJECTILE_OFFSET_Y = 1f;
        private int _terrainMask;

        public virtual void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _terrainMask = LayerMask.GetMask("Terrain");
        }

        public void SetDependencies(IProjectileManager projectileManager)
        {
            _projectileManager = projectileManager;
        }

        public void SetAbility(AbilityBase ability)
        {
            _ability = ability;
        }

        public void SetSource(IUnit source)
        {
            _source = source;
        }

        public void SetSpeed(float speed)
        {
            if (speed < 0)
                return;

            _speed = speed;
        }

        public void SetScale(Vector3 scale)
        {
            transform.localScale = scale;
        }

        public async UniTask MoveToPoint(Vector3 point)
        {
            var cancellToken = this.destroyCancellationToken;
            var isReached = false;

            while (cancellToken.IsCancellationRequested == false && isReached == false)
            {
                var newPosition = GetNewPosition(point);
                MovePositionAlongSurface(newPosition);
                await UniTask.WaitForFixedUpdate(cancellToken);
                isReached = _rigidbody.position.GetXZ().Equals(point.GetXZ());
            }
        }

        public async UniTask MoveToDirection(Vector3 direction)
        {
            direction = direction.normalized;
        }

        private void OnTriggerEnter(Collider other)
        {
            OnHit(other).Forget();
        }

        private async UniTaskVoid OnHit(Collider other)
        {
            var shouldDestroy = await _ability.OnProjectileHit();
            if (shouldDestroy)
                _projectileManager.DestoryProjectile(this);
        }

        private Vector3 GetNewPosition(Vector3 destination)
        {
            var myPosition = _rigidbody.position;
            myPosition.y = 0f;
            destination.y = 0f;
            var newPosition = Vector3.MoveTowards(myPosition, destination, _speed * Time.fixedDeltaTime);
            return newPosition;
        }

        private void MovePositionAlongSurface(Vector3 position)
        {
            var newY = (GetSurfaceY() + PROJECTILE_OFFSET_Y) * transform.localScale.y;
            position.y = newY;
            _rigidbody.MovePosition(position);
        }

        private float GetSurfaceY()
        {
            var raycastPosition = new Vector3(_rigidbody.position.x, RAYCAST_Y, _rigidbody.position.z);
            var successful = Physics.Raycast(raycastPosition, Vector3.down, out RaycastHit raycastHit, 200f, _terrainMask);
            return successful ? raycastHit.point.y : DEFAULT_LEVEL_Y;
        }
    }
}