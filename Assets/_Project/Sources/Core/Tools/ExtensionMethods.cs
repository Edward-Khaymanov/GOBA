using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace GOBA
{
    public static class ExtensionMethods
    {
        public static IList<T> GetValues<T>(this NetworkList<T> networkList) where T : unmanaged, IEquatable<T>
        {
            var list = new List<T>(networkList.Count);
            for (int i = 0; i < networkList.Count; i++)
            {
                list.Add(networkList[i]);
            }
            return list;
        }

        public static Vector2 GetXZ(this Vector3 vector)
        {
            return new Vector2(vector.x, vector.z);
        }

        public static float FlatDistanceTo(this Vector3 from, Vector3 to)
        {
            var a = from.GetXZ();
            var b = to.GetXZ();
            return Vector2.Distance(a, b);
        }

        public static bool HasAnyFlag<TEnum>(this TEnum value, TEnum flags) where TEnum : Enum
        {
            // Cast the enum to its underlying type (int or other numeric type) to do the bitwise comparison
            var valueAsInt = Convert.ToInt64(value);
            var flagsAsInt = Convert.ToInt64(flags);

            // Perform the bitwise AND and check if any flags are set
            return (valueAsInt & flagsAsInt) != 0;
        }

        public static bool IsIEnumerable(this Type type)
        {
            return
                type.Name != nameof(String) &&
                type.GetInterface(nameof(IEnumerable)) != null;
        }
    }
}