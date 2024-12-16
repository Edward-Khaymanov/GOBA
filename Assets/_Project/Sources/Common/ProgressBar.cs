using UnityEngine;
using UnityEngine.UI;

namespace GOBA
{
    public class ProgressBar : MonoBehaviour
    {
        [SerializeField] private Image _mask;
        [SerializeField] private Image _fillImage;
        [SerializeField] private Image _backImage;

        public void SetFillColor(Color color)
        {
            _fillImage.color = color;
        }

        public void SetBackColor(Color color)
        {
            _backImage.color = color;
        }

        public void Fill(float current, float total)
        {
            total = Mathf.Clamp(total, 0f, float.MaxValue);
            current = Mathf.Clamp(current, 0f, total);

            var amount = current == 0f ? 0f : current / total;
            _mask.fillAmount = amount;
        }
    }
}