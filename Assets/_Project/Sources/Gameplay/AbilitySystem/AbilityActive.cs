using Cysharp.Threading.Tasks;
using MapModCore;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GOBA
{
    public abstract class AbilityActive : AbilityBase
    {
        //[field: SerializeField] public AnimationClip ActivateAnimation { get; private set; }
        //[field: SerializeField] public ParticleSystem ActivateParticle { get; private set; }
        //[field: SerializeField] public AnimationClip UseAnimation { get; private set; }
        //[field: SerializeField] public ParticleSystem UseParticle { get; private set; }

        //protected CancellationTokenSource CancelTokenSource = new CancellationTokenSource();

        [field: SerializeField] public AbilityActiveData Data { get; protected set; }
        public bool IsOnCooldown { get; protected set; }
        public bool CanUse => IsOnCooldown == false;


        public abstract UniTask Activate();
        public abstract UniTask Deactivate();


        public async UniTask Use(AbilityCastData castData)
        {
            if (Data.TargettingData.TargetType == AbilityTargetType.Point)
            {
                if (castData.CastPoint == default && Data.TargettingData.TargettingType == AbilityTargettingType.None)
                {
                    castData.CasterReference.TryGet(out Unit caster);
                    castData.CastPoint = caster.transform.position;
                }

                await UseAtPoint(castData.CastPoint);
            }
            else
            {
                var units = new List<Unit>();

                foreach (var unitRef in castData.UnitsReferences)
                {
                    if (unitRef.TryGet(out Unit unit))
                        units.Add(unit);
                }
                await UseAtUnits(units);
            }
        }

        protected virtual async UniTask UseAtPoint(Vector3 point)
        {
            throw new NotImplementedException();
        }

        protected virtual async UniTask UseAtUnits(List<Unit> units)
        {
            throw new NotImplementedException();
        }

        protected async UniTaskVoid WaitCooldown()
        {
            MyLogger.Log("WaitCooldown");
            IsOnCooldown = true;
            await UniTask.Delay(TimeSpan.FromSeconds(Data.CooldownInSeconds));
            IsOnCooldown = false;
            MyLogger.Log("after cooldown");
        }
    }
}