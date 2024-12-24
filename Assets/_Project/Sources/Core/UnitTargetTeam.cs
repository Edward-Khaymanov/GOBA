using System;

namespace GOBA.CORE
{
    [Flags]
    public enum UnitTargetTeam
    {
        NONE        = 0,
        ALL         = ~0,
        FRIENDLY    = 1 << 0,
        ENEMY       = 1 << 1,
    }
}