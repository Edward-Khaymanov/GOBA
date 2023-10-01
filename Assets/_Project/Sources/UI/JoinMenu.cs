using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GOBA
{
    public class JoinMenu : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private TMP_InputField _ipInput;
        [SerializeField] private Button _joinButton;


        private void OnEnable()
        {
            _joinButton.onClick.AddListener(Join);
        }

        private void OnDisable()
        {
            _joinButton.onClick.RemoveListener(Join);
        }

        public void Show()
        {
            _canvas.enabled = true;
            _ipInput.text = CONSTANTS.DEFAULT_IP;
        }

        public void Hide()
        {
            _canvas.enabled = false;
        }

        private void Join()
        {
            new ConnectionManager().StartClient(_ipInput.text, CONSTANTS.DEFAULT_PORT);
        }
    }
}