using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace GOBA
{
    public class TargetProjector : BaseDecalProjector
    {
        private Camera _inputCamera;
        private Texture2D _defaultCursorTexture;

        public void Constructor(Camera inputCamera, Texture2D defaultCursorTexture)
        {
            _inputCamera = inputCamera;
            _defaultCursorTexture = defaultCursorTexture;
        }

        public override void Hide()
        {
            base.Hide();
            _projector.transform.position = Vector3.zero;
            //ChangeCursor();
        }

        public async UniTaskVoid FollowMouse(LayerMask followMask, /*LayerMask hitMask,*/ CancellationToken cancellationToken)
        {
            while (cancellationToken.IsCancellationRequested == false)
            {
                var mouse = Input.mousePosition;
                var castPoint = _inputCamera.ScreenPointToRay(mouse);
                //var mouseOnTarget = false;
                if (Physics.Raycast(castPoint, out RaycastHit hit, CONSTANTS.RAYCAST_MAX_DISTANCE, followMask))
                {
                    transform.position = hit.point;
                    //if (hit.transform.gameObject.layer == hitMask)
                    //    ChangeCursor(new Texture2D(20, 20));
                    //else
                    //    ChangeCursor();
                }

                await UniTask.NextFrame();
            }

        }

        public IEnumerable<T> GetObjectsInRange<T>(LayerMask targetLayers) where T : Component
        {
            var result = new List<T>();
            var colliders = GetCollidersInRange(Radius, targetLayers);

            foreach (var collider in colliders)
            {
                if (collider.gameObject.TryGetComponent<T>(out var comp))
                    result.Add(comp);
            }

            return result;
        }

        public T GetObject<T>(Ray ray, LayerMask targetLayers) where T : Component
        {
            if (Physics.Raycast(ray, out RaycastHit hit, CONSTANTS.RAYCAST_MAX_DISTANCE, targetLayers))
            {
                return hit.collider.gameObject.GetComponent<T>();
            }

            return default;
        }


        private Collider[] GetCollidersInRange(float range, LayerMask layerMask)
        {
            var highPoint = transform.position;
            highPoint.y = 1000;

            var lowPoint = transform.position;
            lowPoint.y = -1000;

            return Physics.OverlapCapsule(highPoint, lowPoint, range, layerMask);
        }



        private void ChangeCursor(Texture2D texture = null)
        {
            if (texture == null)
                Cursor.SetCursor(_defaultCursorTexture, Vector2.zero, CursorMode.ForceSoftware);
            else
                Cursor.SetCursor(texture, Vector2.zero, CursorMode.ForceSoftware);
        }
    }
}