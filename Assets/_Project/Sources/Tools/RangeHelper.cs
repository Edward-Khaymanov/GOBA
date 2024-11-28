using GOBA.CORE;
using UnityEngine;

namespace GOBA
{
    public static class RangeHelper
    {
        //public static bool IsInRange(Vector3 center, float radius, Transform target)
        //{
        //    var result = false;
        //    var colliders = GetCollidersInRange(center, radius, Physics.AllLayers);

        //    foreach (var collider in colliders)
        //    {
        //        result = collider.gameObject == target.gameObject;
        //        if (result)
        //            break;
        //    }

        //    return result;
        //}

        public static bool IsInRange(Vector3 point, float radius, Vector3 target)
        {
            var distance = point.FlatDistanceTo(target);
            return distance <= radius;
        }

        public static bool IsInRange(Vector3 point1, float radius1, Vector3 point2, float radius2, Vector3 target)
        {
            var result = false;

            var distance1 = point1.FlatDistanceTo(point2);
            result = distance1 <= radius1;

            if (result == false)
                return result;

            var distance2 = point2.FlatDistanceTo(target);
            result = distance2 <= radius2;

            return result;
        }

        //private static Collider[] GetCollidersInRange(Vector3 center, float radius, LayerMask layerMask)
        //{
        //    var highPoint = center;
        //    highPoint.y = 100;

        //    var lowPoint = center;
        //    lowPoint.y = -100;

        //    return Physics.OverlapCapsule(highPoint, lowPoint, radius, layerMask);
        //}

        public static int GetCollidersInRange(Vector3 center, float radius, Collider[] colliders, LayerMask layerMask)
        {
            var highPoint = center;
            highPoint.y = 100;

            var lowPoint = center;
            lowPoint.y = -100;

            var hitCount = Physics.OverlapCapsuleNonAlloc(highPoint, lowPoint, radius, colliders, layerMask);
            return hitCount;
        }



        //public static IEnumerable<T> GetUnitsInRange<T>(
        //    Vector3 highPoint,
        //    Vector3 lowPoint,
        //    float radius,
        //    //AbilityUnitTargetType unitType, 
        //    AbilityTargetTeam targetTeam, 
        //    int[] friendyTeams) where T : Unit
        //{
        //    var result = new List<T>();
        //    var colliders = new Collider[CONSTANTS.COLLIDERS_MAX_GET];
        //    Physics.OverlapCapsuleNonAlloc(highPoint, lowPoint, radius, colliders);

        //    foreach (var collider in colliders)
        //    {
        //        if (collider.gameObject.TryGetComponent(out T unit))
        //        {
        //            var isFriendly = friendyTeams.Contains(unit.TeamId);
        //            if (isFriendly && (targetTeam != AbilityTargetTeam.Friendly || targetTeam != AbilityTargetTeam.Any))
        //                continue;

        //            result.Add(unit);
        //        }
        //    }

        //    return result;
        //}
    }
}