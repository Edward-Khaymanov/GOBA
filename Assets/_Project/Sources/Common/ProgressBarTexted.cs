using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GOBA
{
    public class ProgressBarTexted : MonoBehaviour
    {
        [SerializeField] private Image _mask;
        [SerializeField] private TMP_Text _textSource;

        public void Fill(float current, float total, string text)
        {
            total = Mathf.Clamp(total, 0f, int.MaxValue);
            current = Mathf.Clamp(current, 0f, total);

            var amount = current / total;
            if (current == 0f)
            {
                amount = 0f;
            }

            _mask.fillAmount = amount;
            _textSource.text = text;
        }
    }
}