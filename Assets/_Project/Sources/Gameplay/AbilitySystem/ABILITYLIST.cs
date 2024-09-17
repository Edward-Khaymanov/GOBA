using AnimeHeroBattle;
using Cysharp.Threading.Tasks;
using MapModCore;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GOBA
{
    public static class ABILITYLIST
    {
        public static AbilityProjectile _capsuleTemplate;

        public static async UniTask Initialize()
        {
            var capsuleTemplate = await Addressables.LoadAssetAsync<GameObject>("CapsuleTemplate");
            _capsuleTemplate = capsuleTemplate.GetComponent<AbilityProjectile>();
        }

        public static IAbility GetAbility(int id)
        {
            var data = new FireBallData()
            {
                ID = 1,
                CastRadius = 2,
                CastRange = 6,
                Cooldown = 1,
                ProjectileSpeed = 1,
            };
            return new FireBall(data, null, _capsuleTemplate);
        }

        public static IEnumerable<IAbility> GetAbilities()
        {
            return default;
        }
    }
}