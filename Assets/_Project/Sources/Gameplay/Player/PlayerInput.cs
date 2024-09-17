using GOBA.Assets._Project.Sources._Test;
using UnityEngine;

namespace GOBA
{
    public class PlayerInput : MonoBehaviour
    {
        //private LayerMask _terrainMask;

        private bool _initialized;
        private PlayerCamera _playerCamera;
        private Player _player;
        private RaycastHit[] _mouseHits;
        private int _mouseHitsCount;

        public void Init(PlayerCamera playerCamera, Player player)
        {
            _playerCamera = playerCamera;
            _player = player;
            _initialized = true;
            _mouseHits = new RaycastHit[100];
        }

        private void Update()
        {
            if (_initialized == false)
                return;

            RaycastMousePosition();

            if (Input.mouseScrollDelta.y != 0)
            {
                _playerCamera.ChangeHeight(Input.mouseScrollDelta.y);
            }

            if (Input.GetKeyDown(KeyCode.F1))
            {
                _playerCamera.Track(_player.SelectedUnit.Transform);
            }

            if (Input.GetKeyDown(KeyCode.F2))
            {
                _playerCamera.Untrack();
            }

            if (Utils.IsMouseOverUI() == false)
            {
                HandleMovement();
            }
        }

        private void HandleMovement()
        {
            if (Input.GetMouseButtonDown(1))
            {
                var position = GetMovePosition();
                if (position != default)
                {
                    foreach (var unit in _player.SelectedUnits)
                    {
                        var command = new MoveCommand(unit, position);
                        unit.AddCommand(command);
                        //unit.MoveTo(position);
                    }
                }
            }
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

        //private bool GetMovePoint(out Vector3 movePoint)
        //{
        //    movePoint = Vector3.zero;
        //    var mouseScreenPosition = Input.mousePosition;
        //    var ray = _playerCamera.Camera.ScreenPointToRay(mouseScreenPosition);

        //    if (Physics.Raycast(ray, out RaycastHit hit, 200f, _terrainMask) == false)
        //        return false;

        //    movePoint = hit.point;
        //    return true;
        //}

        private void RaycastMousePosition()
        {
            var mouseScreenPosition = Input.mousePosition;
            var ray = _playerCamera.Camera.ScreenPointToRay(mouseScreenPosition);
            _mouseHitsCount = Physics.RaycastNonAlloc(ray, _mouseHits, 100f);
        }
    }
}