using System;

namespace GOBA
{
    public interface IHeroInput
    {
        public event Action<int> AbilityRequested;
        public event Action StopActionRequsted;
        
        public void Enable();
        public void Disable();
    }
}