using GOBA.CORE;
using System;
using Unity.Netcode;
using UnityEngine;

namespace GOBA
{
    public class ParticleManager : IParticleManager
    {
        private IResourceProvider _resourceProvider;
        private ParticleWrapper _particleWrapperTemplate;

        public ParticleManager(IResourceProvider resourceProvider)
        {
            _resourceProvider = resourceProvider;
            _particleWrapperTemplate = _resourceProvider.LoadByKey<ParticleWrapper>("ParticleWrapper");
        }

        public int CreateParticle(string particleName, Vector3 position)
        {
            var particleWrapper = GameObject.Instantiate(_particleWrapperTemplate, position, Quaternion.identity);
            var particleTemplate = _resourceProvider.LoadByKey<NetworkObject>(particleName);
            var particle = GameObject.Instantiate(particleTemplate);
            particleWrapper.NetworkObject.Spawn();
            particle.Spawn();
            particle.TrySetParent(particleWrapper.transform, false);

            return particleWrapper.EntityId;
        }

        public void DestroyParticle(int particleId)
        {

        }
    }
}