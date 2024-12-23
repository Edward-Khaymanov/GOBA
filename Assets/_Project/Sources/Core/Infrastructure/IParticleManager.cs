using UnityEngine;

namespace GOBA.CORE
{
    public interface IParticleManager
    {
        public int CreateParticle(string particleName, Vector3 position);
        public void DestroyParticle(int particleId);
    }
}