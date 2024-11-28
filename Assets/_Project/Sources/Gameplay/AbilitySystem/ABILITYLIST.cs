using AnimeHeroBattle;
using GOBA.CORE;
using System.Collections.Generic;

namespace GOBA
{
    public static class ABILITYLIST
    {
        public static Ability GetAbility(int id)
        {
            var data = new FireBallData()
            {
                Id = 1,
                CastRadius = 2,
                CastRange = 6,
                Cooldown = 4,
                ProjectileSpeed = 1,
            };
            return new FireBall(data, 1);
        }

        public static IEnumerable<Ability> GetAbilities()
        {
            return default;
        }
    }
}