using GOBA.CORE;
using UnityEngine;
using UnityEngine.UI;

namespace GOBA
{
    public class PlayerUI : MonoBehaviour
    {
        [SerializeField] private AbilityUIElement _abilityTemplate;
        [SerializeField] private HorizontalLayoutGroup _abilitiesContainer;
        [SerializeField] private Image _unitIconSource;
        [SerializeField] private ProgressBarTexted _healfBar;
        [SerializeField] private ProgressBarTexted _manaBar;

        private IUnit _target;
        private UnitAsset _targetAsset;
        private readonly bool _targetIsHero;


        private void Update()
        {
            if (_target != null)
            {
                Fill();

            }
        }

        public void OnSelectUnit(IUnit unit)
        {
            _target = unit;
            //_targetAsset = UnitAssetProvider.GetUnit(unit.AssetId);
            //_unitIconSource.sprite = _targetAsset.Icon;
        }

        private void Fill()
        {
            //var data = _target.Stats;

            //_manaBar.Fill(data.Mana.Current, data.Mana.Max, $"{data.Mana.Current} / {data.Mana.Max}");
            //_healfBar.Fill(data.Healf.Current, data.Healf.Max, $"{data.Healf.Current} / {data.Healf.Max}");
        }

        private void FillAbilities()
        {

        }
    }
}