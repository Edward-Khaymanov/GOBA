using Cysharp.Threading.Tasks;
using GOBA.CORE;
using GOBA.DEMO1;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GOBA
{
    public class PlayerInput : MonoBehaviour
    {
        private bool _enabled;
        private int _mouseHitsCount;
        private RaycastHit[] _mouseHits;
        private IList<IUnit> _selectedUnits;
        private PlayerCamera _playerCamera;

        public event Action<IList<IUnit>> UnitsSelected;

        public void Init()
        {
            _playerCamera = PlayerLocalDependencies.PlayerCamera;
            _mouseHits = new RaycastHit[100];
            _selectedUnits = new List<IUnit>();
        }

        public void Enable()
        {
            MyLogger.Log("player input enabled");
            _enabled = true;
        }

        public void Disable()
        {
            _enabled = false;
        }

        private void Update()
        {
            if (_enabled == false)
                return;

            RaycastMousePosition();
            HandleCamera();
            HandleMovement();
            HandleAbilities();

            if (Input.GetKeyDown(KeyCode.S))
            {
                foreach (var unit in _selectedUnits)
                {
                    unit.CancelAction();
                }
            }
        }

        private void HandleCamera()
        {
            if (Input.GetKeyDown(KeyCode.F1))
                _playerCamera.Track(_selectedUnits[0].Transform);

            if (Input.GetKeyDown(KeyCode.F2))
                _playerCamera.Untrack();
        }

        private void HandleMovement()
        {
            MyLogger.Log("player input HandleMovement");
            if (Utils.IsMouseOverUI())
                return;

            if (Input.GetMouseButtonDown(1))
            {
                var position = GetMovePosition();
                if (position != default)
                {
                    foreach (var unit in _selectedUnits)
                    {
                        MainCommandSender.Instance.MoveTo(unit, position);
                    }
                }
            }
        }

        private void HandleAbilities()
        {
            MyLogger.Log("player input HandleAbilities");
            var abilityIndex = -1;

            if (Input.GetKeyDown(KeyCode.Q))
            {
                abilityIndex = 0;
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                abilityIndex = 1;
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                abilityIndex = 2;
            }
            else if (Input.GetKeyDown(KeyCode.R))
            {
                abilityIndex = 3;
            }
            else if (Input.GetKeyDown(KeyCode.T))
            {
                abilityIndex = 4;
            }

            if (abilityIndex == -1)
                return;

            var ability = _selectedUnits[0].GetAbilities()[abilityIndex];
            if (ability.GetBehaviour().HasAnyFlag(AbilityBehaviour.DOTA_ABILITY_BEHAVIOR_PASSIVE | AbilityBehaviour.DOTA_ABILITY_BEHAVIOR_AURA))
                return;

            ActivateAbility(ability).Forget();
        }

        private Vector3 GetMovePosition()
        {
            for (int i = 0; i < _mouseHitsCount; i++)
            {
                var hit = _mouseHits[i];
                if (hit.transform.gameObject.layer == LayerMask.NameToLayer(nameof(CONSTANTS.Layers.Terrain)))
                {
                    return hit.point;
                }
            }
            return default;
        }

        private void RaycastMousePosition()
        {
            MyLogger.Log("player input RaycastMousePosition");
            var mouseScreenPosition = Input.mousePosition;
            var ray = _playerCamera.Camera.ScreenPointToRay(mouseScreenPosition);
            _mouseHitsCount = Physics.RaycastNonAlloc(ray, _mouseHits, 100f);
        }

        public void SelectUnits(IList<IUnit> units)
        {
            _selectedUnits = units;
            UnitsSelected?.Invoke(_selectedUnits);
        }

        private async UniTaskVoid ActivateAbility(AbilityBase ability)
        {
            var castData = new AbilityCastData();
            var abilityOwner = ability.GetOwner();

            if (ability is LightningStrikeSolo)
            {
                var enemy = DIContainer.EntityManager.GetUnits().FirstOrDefault(x => x.GetTeam() != abilityOwner.GetTeam() && x.IsDead() == false);
                if (enemy == default)
                {
                    MyLogger.Log("wrong target");
                    return;
                }

                castData.CastPoint = enemy.Transform.position;
                castData.TargetEntityId = enemy.EntityId;
            }

            if (ability is HealSolo)
            {
                var enemy = DIContainer.EntityManager.GetUnits().FirstOrDefault(x => x.GetTeam() != abilityOwner.GetTeam() && x.IsDead() == false);
                if (enemy == default)
                {
                    MyLogger.Log("wrong target");
                    return;
                }

                castData.CastPoint = enemy.Transform.position;
                castData.TargetEntityId = enemy.EntityId;
            }

            MainCommandSender.Instance.UseAbility(_selectedUnits[0], ability.AbilityId, castData);
        }
    }
}