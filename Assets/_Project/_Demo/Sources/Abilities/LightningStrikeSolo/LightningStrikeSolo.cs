using Cysharp.Threading.Tasks;
using GOBA.CORE;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOBA.DEMO1
{
    public class LightningStrikeSolo : AbilityBase
    {
        protected override async UniTask OnSpellStart()
        {
            var owner = GetOwner();
            var target = GetCastTarget();
            //var particleName = GetDefinitionData<string>("ParticleName");
            var damageType = (DamageType)GetDefinitionData<int>("DamageType");
            var damageList = GetDefinitionData<List<float>>("Damage");
            var damage = damageList[GetLevel() - 1];

            //ParticleManager.CreateParticle(particleName, target.Transform.position);
            ServerFunctions.ApplyDamage(target, owner, damage, damageType, this);
        }
    }
}