using Cysharp.Threading.Tasks;
using MapModCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Unity.Netcode;
using UnityEngine;

namespace GOBA
{
    public class AbilityTargetSelector : MonoBehaviour
    {
        [SerializeField] private RangeProjector _rangeProjector;
        [SerializeField] private TargetProjector _targetProjector;

        private Camera _inputCamera;

        public void Constructor(Camera inputCamera)
        {
            _inputCamera = inputCamera;
            _targetProjector.Constructor(inputCamera, null);
        }

        public async UniTask<AbilityCastData> Select(Unit caster, int[] friendlyTeamsId, AbilityTargettingData targettingData, CancellationToken cancellationToken)
        {
            var result = new AbilityCastData();

            _rangeProjector.SetRadius(targettingData.CastRadius);
            _targetProjector.SetRadius(targettingData.CastPointRadius);
            _rangeProjector.Show();

            if (targettingData.TargetType == AbilityTargetType.Point)
            {
                result = await SelectPoint(caster, targettingData, cancellationToken);
            }
            else
            {
                var units = await SelectUnits(caster, friendlyTeamsId, targettingData, cancellationToken);
                result.UnitsReferences = new NetworkBehaviourReference[units.Count];

                for (int i = 0; i < units.Count; i++)
                {
                    result.UnitsReferences[i] = units[i];
                }
            }

            _rangeProjector.Hide();

            return result;
        }


        #region POINT

        private async UniTask<AbilityCastData> SelectPoint(Unit caster, AbilityTargettingData targettingData, CancellationToken cancellationToken)
        {
            var result = new AbilityCastData();
            result.CasterReference = caster;

            switch (targettingData.TargettingType)
            {
                case AbilityTargettingType.Solo:
                    result.CastPoint = await SelectTerrainPoint(cancellationToken);
                    break;
                case AbilityTargettingType.Area:
                    var followTokenSource = new CancellationTokenSource();
                    _targetProjector.FollowMouse(CONSTANTS.Layers.Terrain, followTokenSource.Token).Forget();
                    _targetProjector.Show();
                    result.CastPoint = await SelectTerrainPoint(cancellationToken);
                    followTokenSource.Cancel();
                    _targetProjector.Hide();
                    break;
            }

            return result;
        }

        private async UniTask<Vector3> SelectTerrainPoint(CancellationToken cancellationToken)
        {
            var result = Vector3.zero;
            var hitLayer = CONSTANTS.Layers.Terrain;
            var hitted = false;

            while (cancellationToken.IsCancellationRequested == false)
            {
                await UniTask.WaitUntil(() => Input.GetKeyDown(KeyCode.Mouse0), cancellationToken: cancellationToken);
                var castPoint = _inputCamera.ScreenPointToRay(Input.mousePosition);

                hitted = Physics.Raycast(castPoint, out RaycastHit hit, CONSTANTS.RAYCAST_MAX_DISTANCE, hitLayer);
                if (hitted == false)
                    continue;

                result = hit.point;
                break;
            }

            return result;
        }

        #endregion


        #region UNITS

        public async UniTask<List<Unit>> SelectUnits(Unit caster, int[] friendlyTeamsId, AbilityTargettingData data, CancellationToken cancellationToken)
        {
            var result = new List<Unit>();
            var tempUnit = default(Unit);

            switch (data.TargettingType)
            {
                case AbilityTargettingType.None:
                    result = SelectInstantTargets(caster, friendlyTeamsId, data);
                    break;
                case AbilityTargettingType.Solo:
                    tempUnit = await SelectSoloTarget(caster, friendlyTeamsId, data, cancellationToken);
                    break;
                case AbilityTargettingType.Area:
                    var followTokenSource = new CancellationTokenSource();
                    _targetProjector.Show();
                    _targetProjector.FollowMouse(CONSTANTS.Layers.Terrain, followTokenSource.Token).Forget();
                    result = await SelectAreaTargets(caster, friendlyTeamsId, data, cancellationToken);
                    followTokenSource.Cancel();
                    _targetProjector.Hide();
                    break;
            }

            if (tempUnit != default)
                result.Add(tempUnit);

            return result;
        }


        private List<Unit> SelectInstantTargets(Unit caster, int[] friendlyTeamsId, AbilityTargettingData data)
        {
            var result = new List<Unit>();
            var colliders = new Collider[CONSTANTS.COLLIDERS_MAX_GET];

            var hitCount = RangeHelper.GetCollidersInRange(caster.transform.position, data.CastPointRadius, colliders, CONSTANTS.Layers.Unit);

            for (int i = 0; i < hitCount; i++)
            {
                colliders[i].TryGetComponent(out Unit unit);

                if (RangeHelper.IsInRange(caster.transform.position, data.CastPointRadius, unit.transform.position) == false)
                    continue;

                var canAdd = CanAddUnit(unit, data.TargetType, friendlyTeamsId, data.TargetTeam);

                if (canAdd)
                    result.Add(unit);
            }

            return result;
        }


        private async UniTask<Unit> SelectSoloTarget(Unit caster, int[] friendlyTeamsId, AbilityTargettingData data, CancellationToken cancellationToken)
        {
            while (cancellationToken.IsCancellationRequested == false)
            {
                await UniTask.WaitUntil(() => Input.GetKeyDown(KeyCode.Mouse0), cancellationToken: cancellationToken);
                var castPoint = _inputCamera.ScreenPointToRay(Input.mousePosition);

                var hitted = Physics.Raycast(castPoint, out RaycastHit hit, CONSTANTS.RAYCAST_MAX_DISTANCE, CONSTANTS.Layers.Unit);
                if (hitted == false)
                    continue;

                hit.collider.TryGetComponent(out Unit unit);

                if (RangeHelper.IsInRange(caster.transform.position, data.CastRadius, unit.transform.position) == false)
                    continue;

                var canAdd = CanAddUnit(unit, data.TargetType, friendlyTeamsId, data.TargetTeam);

                if (canAdd)
                    return unit;
            }

            return default;
        }

        private async UniTask<List<Unit>> SelectAreaTargets(Unit caster, int[] friendlyTeamsId, AbilityTargettingData data, CancellationToken cancellationToken)
        {
            var result = new List<Unit>();
            var colliders = new Collider[CONSTANTS.COLLIDERS_MAX_GET];

            while (cancellationToken.IsCancellationRequested == false)
            {
                await UniTask.WaitUntil(() => Input.GetKeyDown(KeyCode.Mouse0), cancellationToken: cancellationToken);
                var castPoint = _inputCamera.ScreenPointToRay(Input.mousePosition);

                var hitted = Physics.Raycast(castPoint, out RaycastHit hit, CONSTANTS.RAYCAST_MAX_DISTANCE, CONSTANTS.Layers.AllLayers);
                if (hitted == false)
                    continue;

                var hitCount = RangeHelper.GetCollidersInRange(hit.point, data.CastPointRadius, colliders, CONSTANTS.Layers.Unit);

                if (hitCount == 0)
                    continue;

                for (int i = 0; i < hitCount; i++)
                {
                    colliders[i].TryGetComponent(out Unit target);

                    if (RangeHelper.IsInRange(caster.transform.position, data.CastRadius, hit.point, data.CastPointRadius, target.transform.position) == false)
                        continue;

                    var canAdd = CanAddUnit(target, data.TargetType, friendlyTeamsId, data.TargetTeam);

                    if (canAdd)
                        result.Add(target);
                }

                if (result.Count != 0)
                    break;
            }

            return result;
        }

        #endregion


        private bool CanAddUnit(Unit unit, AbilityTargetType unitType, int[] friendlyTeamsId, AbilityTargetTeam targetTeam)
        {
            var isTargetType = UnitIsTargetType(unit, unitType);

            if (isTargetType == false)
                return false;

            var isFriendly = friendlyTeamsId.Contains(unit.TeamId);
            var canAdd = TeamIsMatched(targetTeam, isFriendly, unit.IsNeutral);
            return canAdd;
        }

        private bool UnitIsTargetType(Unit unit, AbilityTargetType unitType)
        {
            var isTargetType = false;
            switch (unitType)
            {
                case AbilityTargetType.Unit:
                    isTargetType = true;
                    break;
                case AbilityTargetType.Creep:
                    isTargetType = unit is Creep;
                    break;
                case AbilityTargetType.Hero:
                    isTargetType = unit is Hero;
                    break;
            }
            return isTargetType;
        }

        private bool TeamIsMatched(AbilityTargetTeam targetTeam, bool isFriendly, bool isNeutral)
        {
            if (targetTeam == AbilityTargetTeam.Any)
            {
                return true;
            }

            if (targetTeam == AbilityTargetTeam.Friendly)
            {
                if (isFriendly)
                    return true;

                return false;
            }

            if (targetTeam == AbilityTargetTeam.Neutral)
            {
                if (isNeutral)
                    return true;

                return false;
            }

            if (targetTeam == AbilityTargetTeam.Enemy)
            {
                if (isFriendly == false)
                    return true;

                return false;
            }

            return false;
        }
    }
}