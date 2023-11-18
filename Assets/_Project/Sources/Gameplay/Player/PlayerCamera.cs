using UnityEngine;

namespace GOBA
{
    public class PlayerCamera : MonoBehaviour
    {
        //при проходе по склонам ограничение высоты работает неправильно
        [SerializeField] private float _heightScale;
        [SerializeField] private float _minHeight;
        [SerializeField] private float _maxHeight;

        private Transform _target;
        private Camera _camera;
        private Vector3 _offset;
        private bool _isTracking;

        public Camera Camera => _camera;

        public void Init()
        {
            _camera = GetComponent<Camera>();
        }

        private void LateUpdate()
        {
            if (_isTracking == false)
                return;

            transform.position = _target.position + _offset;
        }

        public void Track(Transform target)
        {
            _target = target;
            SetOffset(target.position);
            _isTracking = true;
        }

        public void Untrack()
        {
            _isTracking = false;
        }

        public void SetOffset(Vector3 targetPosition)
        {
            transform.position = new Vector3(targetPosition.x, transform.position.y, targetPosition.z);
            _offset = transform.position - targetPosition;
        }

        public void ChangeHeight(float direction)
        {
            var offsetHeight = direction * _heightScale;
            var newPosition = transform.position + new Vector3(0, offsetHeight, 0);

            if (newPosition.y < _minHeight)
                return;

            if (newPosition.y > _maxHeight)
                return;

            transform.position = newPosition;
            _offset.y += offsetHeight;
        }

    }
}