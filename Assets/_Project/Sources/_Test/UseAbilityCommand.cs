using GOBA.CORE;

namespace GOBA
{
    public class UseAbilityCommand : ICommand
    {
        private readonly int _abilityId;
        private readonly IUnit _unit;
        private readonly AbilityCastData _castData;

        public UseAbilityCommand(IUnit unit, int abilityId, AbilityCastData castData)
        {
            _unit = unit;
            _abilityId = abilityId;
            _castData = castData;
        }

        public void Execute()
        {
            _unit.UseAbility(_abilityId, _castData);
        }
    }
}