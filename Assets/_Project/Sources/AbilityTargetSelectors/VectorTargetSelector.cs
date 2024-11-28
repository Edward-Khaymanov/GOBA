using Cysharp.Threading.Tasks;
using GOBA.CORE;
using System;
using System.Threading;
using UnityEngine;

namespace GOBA
{
    public class VectorTargetSelector : BaseDecalProjector
    {
        public async UniTask<AbilityCastData> Select(
            IUnit caster,
            //AbilityTargettingData targettingData,
            CancellationToken cancellationToken)
        {
            return new AbilityCastData()
            {
                CastPoint = caster.Transform.position + new Vector3(10, 10, 0),
            };
        }

        public void SetRange(float range)
        {
            var newSize = new Vector3(_projector.size.x, range * 2, _projector.size.z);
            _projector.size = newSize;
        }

        public override void SetRadius(float radius)
        {
            var newSize = new Vector3(radius * 2, _projector.size.y, _projector.size.z);
            _projector.size = newSize;
        }
    }
}