using Unity.Netcode;
using UnityEngine;

namespace GOBA
{
    public class LobbySpawner : MonoBehaviour
    {
        [SerializeField] private Lobby _lobbyTemplate;

        private void Start()
        {
            if (NetworkManager.Singleton.IsServer)
            {
                var lobby = GameObject.Instantiate(_lobbyTemplate);
                lobby.NetworkObject.Spawn();
            }
        }
    }
}
