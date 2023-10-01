using System.Collections.Generic;
using UnityEngine;

namespace GOBA
{
    public class Player : MonoBehaviour
    {
        //public int _ownerUserId;
        private Unit _selectedUnit;
        private PlayerInput _playerInput;
        private PlayerCamera _playerCamera;
        private PlayerUI _playerUI;

        public List<Unit> _ownedUnits = new List<Unit>();

        public void Init()
        {
            _playerCamera = FindObjectOfType<PlayerCamera>();
            _playerCamera.Init();

            _playerInput = FindObjectOfType<PlayerInput>();
            _playerInput.Init(_playerCamera.Camera);

            _playerUI = FindObjectOfType<PlayerUI>();

            SubscribeToEvents();
        }

        public void AddUnit(Unit unit)
        {
            _ownedUnits.Add(unit);
        }

        public void SelectUnit(Unit unit)
        {
            if (_ownedUnits.Contains(unit))
                _selectedUnit = unit;

            _playerUI.OnSelectUnit(unit);
        }

        private void Track()
        {
            _playerCamera.Track(_selectedUnit.transform);
        }

        private void MoveSelected(Vector3 targetPoint)
        {
            _selectedUnit.Move(targetPoint);
        }

        private void SubscribeToEvents()
        {
            _playerInput.Moving += MoveSelected;
            _playerInput.Scroling += _playerCamera.ChangeHeight;
            _playerInput.Tracking += Track;
            _playerInput.Untracking += _playerCamera.Untrack;
        }

        private void OnDisable()
        {
            _playerInput.Moving -= _selectedUnit.Move;
            _playerInput.Scroling -= _playerCamera.ChangeHeight;
            _playerInput.Tracking -= Track;
            _playerInput.Untracking -= _playerCamera.Untrack;
        }
    }
}