using System;
using UnityEngine;

namespace GOBA
{
    public class PlayerInput : MonoBehaviour
    {
        [SerializeField] private LayerMask _moveMask;

        private Camera _camera;

        public event Action<Vector3> Moving;
        public event Action Tracking;
        public event Action Untracking;
        public event Action<float> Scroling;

        public void Init(Camera playerCamera)
        {
            _camera = playerCamera;
        }


        private void Update()
        {
            if (Input.GetMouseButtonDown(1) && GetMovePoint(out Vector3 movePoint))
            {
                Moving?.Invoke(movePoint);
            }

            if (Input.mouseScrollDelta.y != 0)
            {
                Scroling?.Invoke(Input.mouseScrollDelta.y);
            }

            if (Input.GetKeyDown(KeyCode.F1))
            {
                Tracking?.Invoke();
            }

            if (Input.GetKeyDown(KeyCode.F2))
            {
                Untracking?.Invoke();
            }
        }

        private bool GetMovePoint(out Vector3 movePoint)
        {
            movePoint = Vector3.zero;
            var mouseScreenPosition = Input.mousePosition;
            var ray = _camera.ScreenPointToRay(mouseScreenPosition);

            if (Physics.Raycast(ray, out RaycastHit hit, 200f, _moveMask) == false)
                return false;

            movePoint = hit.point;
            return true;
        }
    }
}