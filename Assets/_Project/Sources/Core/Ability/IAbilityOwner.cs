﻿using System.Collections.Generic;

namespace GOBA.CORE
{
    public interface IAbilityOwner
    {
        public IList<Ability> GetAbilities();
    }
}