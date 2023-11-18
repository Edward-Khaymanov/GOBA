using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GOBA
{
    public class HeroView : MonoBehaviour
    {
        private Animator _animator;
        private AnimatorOverrideController _overrideAnimator;
        private List<KeyValuePair<AnimationClip, AnimationClip>> _overrides;

        public void Init(UnitView unitView)
        {
            Instantiate(unitView, transform);
            _overrides = new List<KeyValuePair<AnimationClip, AnimationClip>>();
            _animator = GetComponentInChildren<Animator>();
            _overrideAnimator = new AnimatorOverrideController(_animator.runtimeAnimatorController);
            _animator.runtimeAnimatorController = _overrideAnimator;
        }

        public void SetTrigger(string triggerName)
        {
            _animator.SetTrigger(triggerName);
        }

        public void OverrideAnimation(string overrideClipName, AnimationClip clip)
        {
            _overrideAnimator.GetOverrides(_overrides);
            var target = _overrides.FirstOrDefault(x => x.Key.name == overrideClipName);
            var index = _overrides.IndexOf(target);
            _overrides[index] = new KeyValuePair<AnimationClip, AnimationClip>(_overrides[index].Key, clip);
            _overrideAnimator.ApplyOverrides(_overrides);
            _animator.runtimeAnimatorController = _overrideAnimator;
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
