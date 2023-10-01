using UnityEngine;

namespace GOBA
{
    public class RangeProjector : BaseDecalProjector
    {
        //public bool ObjectInRange(Vector3 objectCenter, float objectRadius)
        //{
        //    _projector = GetComponent<DecalProjector>();

        //    var distance = transform.position.FlatDistanceTo(objectCenter);
        //    var myradius = _projector.size.x / 2;
        //    return distance <= myradius + objectRadius;
        //}


        public bool IsInRange<T>(T target) where T : Component
        {
            var result = false;
            var colliders = GetCollidersInRange(Radius, Physics.AllLayers);

            foreach (var collider in colliders)
            {
                result = collider.gameObject == target.gameObject;
                if (result)
                    break;
            }

            return result;
        }

        public bool IsInRange(Vector3 point)
        {
            var distance = transform.position.FlatDistanceTo(point);
            var myradius = _projector.size.x / 2;
            return distance <= myradius;
        }

        private Collider[] GetCollidersInRange(float range, LayerMask layerMask)
        {
            var highPoint = transform.position;
            highPoint.y = 1000;

            var lowPoint = transform.position;
            lowPoint.y = -1000;

            return Physics.OverlapCapsule(highPoint, lowPoint, range, layerMask);
        }
    }
}