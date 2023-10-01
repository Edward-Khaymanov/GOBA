using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOBA
{
    public class LookAt : MonoBehaviour
    {
        private Transform _target;
        // Start is called before the first frame update
        void Start()
        {
            _target = Camera.main.transform;
        }

        // Update is called once per frame
        void Update()
        {
            //transform.LookAt(_target.position + _target.forward);
            transform.LookAt(transform.position + _target.rotation * Vector3.back, _target.rotation * Vector3.up);
        }
    }
}
