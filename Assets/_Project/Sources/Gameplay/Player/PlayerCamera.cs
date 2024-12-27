using GOBA.CORE;
using UnityEngine;

namespace GOBA
{
    public class PlayerCamera : MonoBehaviour
    {
        [SerializeField, Min(MIN_OFFSET_DISTANCE)] private float _offsetDistance;
        [SerializeField] private float _smoothTime;
        [SerializeField] private Camera _camera;

        private Transform _target;
        private bool _isTracking;
        private float _rotationX = 70f;

        private const float MIN_OFFSET_DISTANCE = 10f;

        public Camera Camera => _camera;

        public void Init()
        {
            SetRotation(new Vector3(_rotationX, 0, 0));
        }

        private void LateUpdate()
        {
            if (_isTracking == false)
                return;

            if (_offsetDistance < MIN_OFFSET_DISTANCE)
                _offsetDistance = MIN_OFFSET_DISTANCE;

            transform.position = Vector3.Lerp(transform.position, GetCameraCenterPosition(_target.position), _smoothTime);
        }

        public void Track(Transform target)
        {
            _target = target;
            _isTracking = true;
        }

        public void Untrack()
        {
            _isTracking = false;
        }

        public void CenterCameraOnUnit(IUnit unit)
        {
            Untrack();
            transform.position = GetCameraCenterPosition(unit.Transform.position);
        }

        public void SetRotation(Vector3 rotation)
        {
            transform.rotation = Quaternion.Euler(rotation);
        }

        public Vector3 GetCameraCenterPosition(Vector3 position)
        {
            return position + (-_camera.transform.forward * _offsetDistance);
        }
    }
}