using GOBA.DEMO1;
using Cysharp.Threading.Tasks;
using GOBA.Assets._Project.Sources._Test;
using GOBA.CORE;
using System.Collections.Generic;
using UnityEngine;

namespace GOBA
{
    public class PlayerInput : MonoBehaviour
    {
        private bool _enabled;
        private int _mouseHitsCount;
        private RaycastHit[] _mouseHits;
        private IList<IUnit> _selectedUnits;//unit group в дальнейшем?
        private PlayerCamera _playerCamera;
        private PlayerUI _playerUI;

        public void Init(PlayerCamera playerCamera, PlayerUI playerUI)
        {
            _playerCamera = playerCamera;
            _playerUI = playerUI;
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
            MyLogger.Log("player input Update");

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
            MyLogger.Log("player input HandleCamera");
            if (Input.mouseScrollDelta.y != 0)
                _playerCamera.ChangeHeight(Input.mouseScrollDelta.y);

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
            _playerUI.OnSelectUnit(_selectedUnits[0]);
        }

        private async UniTaskVoid ActivateAbility(AbilityBase ability)
        {
            //выброр цели
            var castData = new AbilityCastData();
            var abilityOwner = ability.GetOwner();

            if (ability is FireBall)
            {
                castData = new AbilityCastData()
                {
                    CastPoint = abilityOwner.Transform.position + new Vector3(10, 10, 10),
                    //UnitsReferences = new NetworkBehaviourReference [] { _selectedUnits[0].NetworkBehaviour }
                };
            }

            MainCommandSender.Instance.UseAbility(_selectedUnits[0], ability.AbilityId, castData);
        }
    }
}