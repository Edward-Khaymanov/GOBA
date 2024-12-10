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
    }
}