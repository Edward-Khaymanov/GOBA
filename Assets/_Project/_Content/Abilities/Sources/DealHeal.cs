using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GOBA
{
    public class DealHeal : AbilityActive
    {
        [SerializeField] private float _healAmount;

        public override async UniTask Activate()
        {
            MyLogger.Log("Activate DealHeal");
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
                hero.TakeHeal(_healAmount);
            }

            WaitCooldown().Forget();
        }
    }
}