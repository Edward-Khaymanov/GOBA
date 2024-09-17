using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GOBA
{
    public class HeroView : MonoBehaviour
    {
        private Animator _animator;
        private AnimatorOverrideController _overrideAnimator;

        public void Init()
        {
            _animator = transform.parent.GetComponentInChildren<Animator>();
            _overrideAnimator = new AnimatorOverrideController(_animator.runtimeAnimatorController);
            _animator.runtimeAnimatorController = _overrideAnimator;
        }

        public void PlayState(string stateName)
        {
            _animator.Play(stateName, 0);
        }

        public ParticleSystem PlayParticle(ParticleSystem particle, Vector3 position, Transform parent)
        {
            var currentParticle = GameObject.Instantiate(particle, position, Quaternion.identity, parent);
            currentParticle.Play();
            return currentParticle;
        }

        public void StopParticle(ParticleSystem particle)
        {
            GameObject.Destroy(particle.gameObject);
        }
    }
}
