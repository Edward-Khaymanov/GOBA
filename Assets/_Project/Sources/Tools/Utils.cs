using System;
using UnityEngine.EventSystems;

namespace GOBA
{
    public static class Utils
    {
        public static bool IsMouseOverUI()
        {
            //var sd = new List<RaycastResult>();
            //EventSystem.current.RaycastAll()
            return EventSystem.current.IsPointerOverGameObject();
        }

        public static bool HasAnyFlag<TEnum>(TEnum value, TEnum flags) where TEnum : Enum
        {
            // Cast the enum to its underlying type (int or other numeric type) to do the bitwise comparison
            var valueAsInt = Convert.ToInt64(value);
            var flagsAsInt = Convert.ToInt64(flags);

            // Perform the bitwise AND and check if any flags are set
            return (valueAsInt & flagsAsInt) != 0;
        }
    }
}