using MapModCore;
using System;

namespace GOBA.Assets._Project.Sources._Test
{
    public class UseAbilityCommand : ICommand
    {
        private readonly IUnit _unit;
        private readonly int _abilityIndex;

        public UseAbilityCommand(IUnit unit, int abilityIndex)
        {
            _unit = unit;
            _abilityIndex = abilityIndex;
        }

        public void Execute()
        {
            _unit.UseAbility(_abilityIndex);
        }
    }
}