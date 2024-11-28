using GOBA.CORE;
using System;
using Unity.Netcode;

namespace AnimeHeroBattle
{
    [Serializable]
    public struct FireBallData : INetworkSerializeByMemcpy
    {
        public int Id;
        public AbilityBehaviour Behaviour;
        public AbilityUnitTargetType UnitTargetType;
        public AbilityUnitTargetTeam UnitTargetTeam;
        public float CastRange;
        public float CastRadius;
        public CostType CostType;
        public float Cost;

        public DamageType DamageType;
        public float DamageAmount;
        public float CastDelay;
        public float Cooldown;

        public float ProjectileSpeed;
    }
}