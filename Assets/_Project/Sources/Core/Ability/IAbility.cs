//using System;
//using Unity.Netcode;

//namespace GOBA.CORE
//{
//    public interface IAbility
//    {
//        //public AbilityTargettingData TargettingData { get; }
//        //public AbilityTargettingType TargettingType;
//        //public AbilityUnitTargetFlags Flags { get; }
//        public int Id { get; }
//        public int OwnerEntityId { get; }
//        public AbilityBehaviour Behaviour { get; }
//        public AbilityUnitTargetType UnitTargetType { get; }
//        public AbilityUnitTargetTeam UnitTargetTeam { get; }

//        public void SetOwner(int ownerEntityId);
//        public void Use(AbilityCastData castData);
//        public float GetCooldownTimeRemaining();
//        public float GetCooldown(int level);
//        public void StartCooldown(float cooldown);
//        public void EndCooldown();
//    }
//}