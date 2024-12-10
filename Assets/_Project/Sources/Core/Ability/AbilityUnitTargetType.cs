using System;

namespace GOBA.CORE
{
    [Flags]
    public enum AbilityUnitTargetType
    {
        //NONE    = 1 << 0,
        //ALL     = 1 << 1,
        //BASIC   = 1 << 2,
        //HERO    = 1 << 3,
        DOTA_UNIT_TARGET_NONE                       = 1 << 0,
        DOTA_UNIT_TARGET_HERO                       = 1 << 1,
        DOTA_UNIT_TARGET_CREEP                      = 1 << 2,
        DOTA_UNIT_TARGET_BUILDING                   = 1 << 3,
        DOTA_UNIT_TARGET_COURIER                    = 1 << 4,
        DOTA_UNIT_TARGET_BASIC                      = 1 << 5,
        //DOTA_UNIT_TARGET_HEROES_AND_CREEPS          = 1 << 6,
        DOTA_UNIT_TARGET_OTHER                      = 1 << 7,
        DOTA_UNIT_TARGET_ALL                        = 1 << 8,
        DOTA_UNIT_TARGET_TREE                       = 1 << 9,
        DOTA_UNIT_TARGET_CUSTOM                     = 1 << 10,
        DOTA_UNIT_TARGET_SELF                       = 1 << 11,
    }
}