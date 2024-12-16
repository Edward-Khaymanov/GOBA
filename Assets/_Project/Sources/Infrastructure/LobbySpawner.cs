using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace GOBA
{
    /// <summary>
    /// Need spawner because NGO have some problems with in-scene placed network objects
    /// </summary>
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
