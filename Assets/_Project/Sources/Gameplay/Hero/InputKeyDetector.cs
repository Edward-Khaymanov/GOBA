using System;
using System.Collections.Generic;
using UnityEngine;

namespace GOBA
{
    public class InputKeyDetector : IUpdate
    {
        private readonly IEnumerable<KeyCode> _hotKeys;

        public event Action<KeyCode> HotKeyPressed;

        public InputKeyDetector(IEnumerable<KeyCode> hotKeys)
        {
            _hotKeys = hotKeys;
        }

        public void Update()
        {
            if (Input.anyKeyDown == false)
                return;

            foreach (KeyCode key in _hotKeys)
            {
                if (Input.GetKeyDown(key))
                    HotKeyPressed?.Invoke(key);
            }
        }
    }
}
