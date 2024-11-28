using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace GOBA
{
    public abstract class BaseDecalProjector : MonoBehaviour
    {
        protected DecalProjector _projector;

        public float Radius => _projector.size.x / 2;

        protected virtual void Awake()
        {
            _projector = GetComponent<DecalProjector>();
        }

        public virtual void Show()
        {
            _projector.enabled = true;
        }

        public virtual void Hide()
        {
            _projector.enabled = false;
        }

        public virtual void SetRadius(float radius)
        {
            var newSize = new Vector3(radius * 2, radius * 2, _projector.size.z);
            _projector.size = newSize;
        }
    }
}