using Cysharp.Threading.Tasks;
using GOBA.CORE;
using System.Collections.Generic;
using UnityEngine;

namespace GOBA.DEMO1
{
    public class LightningStrikeAll : AbilityBase
    {
        protected override async UniTask OnSpellStart()
        {
            var owner = GetOwner();
            var particleName = GetDefinitionData<string>("ParticleName");
            var damageType = (DamageType)GetDefinitionData<int>("DamageType");
            var damageList = GetDefinitionData<List<float>>("Damage");
            var damage = damageList[GetLevel() - 1];
            var targetTeam = GetTargetTeam();
            var targetType = GetTargetType();
            var targets = ServerFunctions.FindUnits(owner.GetTeam(), targetTeam, targetType);

            foreach (var target in targets)
            {
                ParticleManager.CreateParticle(particleName, target.Transform.position);
                ServerFunctions.ApplyDamage(target, owner, damage, damageType, this);
            }
        }
    }
}