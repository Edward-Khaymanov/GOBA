using System;

namespace GOBA.CORE
{
    [Flags]
    public enum AbilityUnitTargetTeam
    {
        NONE        = 1 << 0,
        ALL         = 1 << 1,
        FRIENDLY    = 1 << 2,
        ENEMY       = 1 << 3,
    }
}