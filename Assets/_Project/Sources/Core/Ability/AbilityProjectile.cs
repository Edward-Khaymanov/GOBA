using Cysharp.Threading.Tasks;
using System.Threading;
using Unity.Netcode;
using UnityEngine;

namespace GOBA.CORE
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    public class AbilityProjectile : NetworkBehaviour
    {
        [SerializeField] private int _id;

        private Rigidbody _rigidbody;
        private CancellationTokenSource _moveCancellationTokenSource = new CancellationTokenSource();
        private float _speed;

        public int Id => _id;

        public virtual void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
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

        public async UniTask MoveTo(Vector3 point)
        {
            _moveCancellationTokenSource = new CancellationTokenSource();
            var cancellToken = _moveCancellationTokenSource.Token;
            var isReached = false;

            while (cancellToken.IsCancellationRequested == false && isReached == false)
            {
                var direction = point - _rigidbody.position;
                var newPosition = _rigidbody.position + _speed * Time.fixedDeltaTime * direction.normalized;
                if (direction.sqrMagnitude < newPosition.sqrMagnitude)
                {
                    newPosition = point;
                }
                _rigidbody.MovePosition(newPosition);
                await UniTask.WaitForFixedUpdate(cancellToken);
                isReached = _rigidbody.position == point;
            }
        }
    }
}