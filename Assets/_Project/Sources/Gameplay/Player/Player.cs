using MapModCore;
using System.Collections.Generic;
using UnityEngine;

namespace GOBA
{
    public class Player : MonoBehaviour
    {
        //public int _ownerUserId;
        private PlayerUI _playerUI;
        public IUnit SelectedUnit { get; private set; }
        public List<IUnit> SelectedUnits { get; private set; }//unit group ג האכüםוירול?

        public void Init(PlayerUI playerUI)
        {
            _playerUI = playerUI;
            SelectedUnits = new List<IUnit>();
        }

        public void SelectUnit(IUnit unit)
        {
            SelectedUnit = unit;
            unit.EnableInput();
            _playerUI.OnSelectUnit(unit);
        }
    }
}