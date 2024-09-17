using System;
using UnityEngine;

namespace GOBA
{
    public interface IHeroInput
    {
        public Vector2 MousePosition { get; }
        
        public void Enable();
        public void Disable();
        public void SetAbilityCallback(Action<int> callback);
        public void SetStopActionCallback(Action callback);
    }
}