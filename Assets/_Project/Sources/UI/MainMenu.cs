using Cysharp.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GOBA
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private Button _createButton;
        [SerializeField] private Button _joinButton;

        private void OnEnable()
        {
            _createButton.onClick.AddListener(Create);
            _joinButton.onClick.AddListener(Join);
            NetworkManager.Singleton.OnServerStarted += OnServerStarted;
        }

        private void OnServerStarted()
        {
            NetworkManager.Singleton.SceneManager.LoadScene(CONSTANTS.LOBBY_SCENE_NAME, LoadSceneMode.Single);
        }

        private void OnDisable()
        {
            _createButton.onClick.RemoveListener(Create);
            _joinButton.onClick.AddListener(Join);
            NetworkManager.Singleton.OnServerStarted -= OnServerStarted;
        }

        public void Show()
        {
            _canvas.enabled = true;
        }

        public void Hide()
        {
            _canvas.enabled = false;
        }

        private void Create()
        {
            new ConnectionManager().StartHost(CONSTANTS.DEFAULT_PORT);
        }

        private void Join()
        {
            new ConnectionManager().StartClient(CONSTANTS.DEFAULT_IP, CONSTANTS.DEFAULT_PORT);
        }
    }
}
