using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace GOBA
{
    public class DealDamage : AbilityActive
    {

        [SerializeField] private DamageType _damageType;
        [SerializeField] private float _damageAmount;

        public override async UniTask Activate()
        {
            MyLogger.Log("Activate DealDamage");
            if (CanUse == false)
                return;

            MyLogger.Log("IsActive");
        }

        public override async UniTask Deactivate()
        {

        }

        protected override async UniTask UseAtUnits(List<Unit> units)
        {
            MyLogger.Log(MethodBase.GetCurrentMethod().Name);
            if (CanUse == false)
                return;

            await UniTask.Delay(TimeSpan.FromSeconds(Data.CastTimeInSeconds)/*, cancellationToken: currentToken*/);

            foreach (var hero in units)
            {
                hero.TakeDamage(_damageType, _damageAmount);
            }

            WaitCooldown().Forget();
        }
    }
}
