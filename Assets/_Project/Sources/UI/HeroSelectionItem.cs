using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GOBA
{
    public class HeroSelectionItem : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Image _iconRenderer;
        [SerializeField] private LayoutElement _layoutElement;

        public event Action<int> HeroSelected;
        public int HeroId { get; private set; }

        public void Init(HeroAsset heroAsset)
        {
            HeroId = heroAsset.Id;
            //_iconRenderer.sprite = heroAsset.Icon;
        }

        public void SetWidth(float value)
        {
            _layoutElement.preferredWidth = value;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            HeroSelected?.Invoke(HeroId);
        }
    }
}