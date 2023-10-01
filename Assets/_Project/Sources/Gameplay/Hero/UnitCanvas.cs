using UnityEngine;

namespace GOBA
{
    public class UnitCanvas : MonoBehaviour
    {
        [SerializeField] private ProgressBar _healfBar;
        [SerializeField] private ProgressBar _manaBar;

        public void OnHealfChanged(float current, float total)
        {
            _healfBar.Fill(current, total);
        }

        public void OnManaChanged(float current, float total)
        {
            _healfBar.Fill(current, total);
        }
    }
}