using System;

namespace GOBA.CORE
{
    [Flags]
    public enum UnitTargetType
    {
        NONE                       = 0,
        ALL                        = ~0,
        BASIC                      = 1 << 0,
        HERO                       = 1 << 1,
    }
}