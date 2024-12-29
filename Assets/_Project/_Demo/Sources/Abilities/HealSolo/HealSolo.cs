using Cysharp.Threading.Tasks;
using GOBA.CORE;
using System.Collections.Generic;

namespace GOBA.DEMO1
{
    public class HealSolo : AbilityBase
    {
        protected override async UniTask OnSpellStart()
        {
            var owner = GetOwner();
            var target = GetCastTarget();
            var particleName = GetDefinitionData<string>("ParticleName");
            var healList = GetDefinitionData<List<float>>("Heal");
            var heal = healList[GetLevel() - 1];

            ParticleManager.CreateParticle(particleName, target.Transform.position);
            ServerFunctions.Heal(target, heal);
        }
    }
}