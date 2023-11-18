using UnityEngine;

namespace GOBA
{
    public class LookAt : MonoBehaviour
    {
        private Transform _target;

        private void Start()
        {
            _target = Camera.main.transform;
        }

        private void Update()
        {
            //transform.LookAt(_target.position + _target.forward);
            transform.LookAt(transform.position + _target.rotation * Vector3.back, _target.rotation * Vector3.up);
        }
    }
}
