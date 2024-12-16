using DG.Tweening;
using GOBA.CORE;
using UnityEngine;

namespace GOBA
{
    public class WorldUnitBar : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private ProgressBar _hpProgressBar;
        [SerializeField] private ProgressBar _manaProgressBar;

        private IUnit _target;
        private bool _isEnabled;

        private void Update()
        {
            UpdateBars();
        }

        public void Init(IUnit unit)
        {
            _target = unit;
            SetColors();
        }

        public void Enable()
        {
            _isEnabled = true;
            //показывать
        }

        public void Disable()
        {
            _isEnabled = false;
            //прятать
        }

        private void SetColors()
        {
            var localPlayerTeam = DIContainer.EntityManager.GetLocalPlayerController().GetPlayerTeam();
            var targetTeam = _target.GetTeam();

            ColorUtility.TryParseHtmlString("#456ddd", out var fillcolor);
            ColorUtility.TryParseHtmlString("#152041", out var backcolor);
            
            _manaProgressBar.SetFillColor(fillcolor);
            _manaProgressBar.SetBackColor(backcolor);

            if (localPlayerTeam == targetTeam)
            {
                _hpProgressBar.SetFillColor(Color.green);
                _hpProgressBar.SetBackColor(Color.green - new Color(0, 0.2f, 0, 0));
            }
        }

        private void UpdateBars()
        {
            if (_isEnabled && _target != default)
            {
                var health = _target.GetHealth();
                var maxHealth = _target.GetMaxHealth();
                _hpProgressBar.Fill(health, maxHealth);

                var mana = _target.GetMana();
                var maxMana = _target.GetMaxMana();
                _manaProgressBar.Fill(mana, maxMana);

                var barsWorldPosition = _target.Transform.position + new Vector3(0f, _target.GetHeight());
                var barsScreenPosition = RectTransformUtility.WorldToScreenPoint(PlayerLocalDependencies.PlayerCamera.Camera, barsWorldPosition);
                _rectTransform.position = barsScreenPosition;
            }
        }

        private void OnUnitChangeTeam(IUnit unit, int teamId)
        {
            if (unit == _target)
                SetColors();
        }
    }
}