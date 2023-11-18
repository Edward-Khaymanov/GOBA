using UnityEngine;
using UnityEngine.UI;

namespace GOBA
{
    public class HeroSelectionRow : MonoBehaviour
    {
        [SerializeField] private LayoutElement _layoutElement;
        [SerializeField] private HorizontalLayoutGroup _layoutGroup;

        public void SetHeight(float value)
        {
            _layoutElement.preferredHeight = value;
        }

        public void SetSpacing(float value)
        {
            _layoutGroup.spacing = value;
        }
    }
}