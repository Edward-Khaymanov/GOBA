using GOBA.CORE;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GOBA
{
    public class BarsRenderer : MonoBehaviour
    {
        [SerializeField] private WorldUnitBar _worldUnitBarTemplate;

        private Dictionary<int, WorldUnitBar> _entityIdBar;
        private bool _isEnabled;

        private void Awake()
        {
            _entityIdBar = new Dictionary<int, WorldUnitBar>();
        }

        private void OnEnable()
        {
            DIContainer.EntityManager.EntitySpawned += OnEntitySpawned;
        }

        private void OnDisable()
        {
            DIContainer.EntityManager.EntitySpawned -= OnEntitySpawned;
        }

        private void Update()
        {
            if (_isEnabled == false)
                return;

            var unitsInVision = GetUnitsInVission().Select(x => x.EntityId);

            foreach (var kv in _entityIdBar)
            {
                if (unitsInVision.Contains(kv.Key))
                {
                    kv.Value.Enable();
                }
                else
                {
                    kv.Value.Disable();
                }
            }
        }

        public void Enable()
        {
            _isEnabled = true;
        }

        public void Disable()
        {
            _isEnabled = false;
            foreach (var bars in _entityIdBar.Values)
            {
                bars.Disable();
            }
        }

        private void OnEntitySpawned(IGameEntity entity)
        {
            if (entity is IUnit unit && _entityIdBar.ContainsKey(unit.EntityId) == false)
            {
                var bar = GameObject.Instantiate(_worldUnitBarTemplate, transform);
                bar.Init(unit);
                _entityIdBar.TryAdd(unit.EntityId, bar);
            }
        }

        private IEnumerable<IUnit> GetUnitsInVission()
        {
            return DIContainer.EntityManager.GetUnits().Where(x => x.IsDead() == false);
        }
    }
}