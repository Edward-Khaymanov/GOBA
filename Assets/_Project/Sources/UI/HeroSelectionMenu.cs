using System;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace GOBA
{
    public class HeroSelectionMenu : NetworkBehaviour
    {
        [Header("References")]
        [SerializeField] private VerticalLayoutGroup _layoutGroup;
        [SerializeField] private Button _confirmButton;
        [SerializeField] private RectTransform _container;

        [Header("Templates")]
        [SerializeField] private HeroSelectionRow _rowTemplate;
        [SerializeField] private HeroSelectionItem _heroTemplate;

        [Header("Settings")]
        [SerializeField] private int _rowHeight = 200;
        [SerializeField] private int _heroWidth = 100;
        [SerializeField] private int _rowSpacing = 10;
        [SerializeField] private int _heroSpacing = 10;


        private int _selectedHeroId = int.MinValue;

        public event Action<ulong, int> ClientHeroSelected;

        public int MaxHeroesInRow => (int)(_container.rect.width + _heroSpacing) / (_heroWidth + _heroSpacing);


        private void OnEnable()
        {
            _confirmButton.onClick.AddListener(ConfirmSelectedHero);
        }

        private void OnDisable()
        {
            _confirmButton.onClick.RemoveListener(ConfirmSelectedHero);
        }

        public void StartSelecting(ClientRpcParams targetClients)
        {
            if (base.IsServer == false)
                return;

            LoadClientRpc(targetClients);
        }

        [ClientRpc]
        private void LoadClientRpc(ClientRpcParams clientRpc)
        {
            var heroes = UnitAssetProvider.GetHeroes();
            var targetRows = Math.Ceiling((double)heroes.Count / MaxHeroesInRow);

            _layoutGroup.spacing = _rowSpacing;

            for (int i = 0; i < targetRows; i++)
            {
                var row = AddRow();
                row.SetHeight(_rowHeight);
                row.SetSpacing(_heroSpacing);

                var rowHeroes = heroes.Skip(i * MaxHeroesInRow).Take((i + 1) * MaxHeroesInRow);

                foreach (var heroAsset in rowHeroes)
                {
                    var selection = AddHero(row.transform);
                    selection.SetWidth(_heroWidth);
                    selection.Init(heroAsset);
                    selection.HeroSelected += OnHeroSelected;
                }
            }
        }

        [ServerRpc]
        private void SelectHeroServerRpc(int heroId, ServerRpcParams serverRpc = default)
        {
            ClientHeroSelected?.Invoke(serverRpc.Receive.SenderClientId, heroId);
        }

        private HeroSelectionRow AddRow()
        {
            return Instantiate(_rowTemplate, _container);
        }

        private HeroSelectionItem AddHero(Transform parent)
        {
            return Instantiate(_heroTemplate, parent);
        }

        private void ConfirmSelectedHero()
        {
            SelectHeroServerRpc(_selectedHeroId);
        }

        private void OnHeroSelected(int heroId)
        {
            _selectedHeroId = heroId;
        }
    }
}
