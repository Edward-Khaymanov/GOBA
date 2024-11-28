using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GOBA
{
    public class ProgressBarTexted : ProgressBar
    {
        [SerializeField] private TMP_Text _textSource;

        public void SetText(string text)
        {
            _textSource.text = text;
        }
    }
}