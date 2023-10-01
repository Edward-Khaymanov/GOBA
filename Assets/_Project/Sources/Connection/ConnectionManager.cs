using Unity.Netcode;
using Unity.Netcode.Transports.UTP;

namespace GOBA
{
    public class ConnectionManager
    {
        public void StartClient(string ipAddress, ushort port)
        {
            InitNetwork(ipAddress, port, null);
            NetworkManager.Singleton.StartClient();
        }

        public void StartHost(ushort port, string listenAddress = "0.0.0.0")
        {
            InitNetwork("127.0.0.1", port, listenAddress);
            NetworkManager.Singleton.StartHost();
        }

        private void InitNetwork(string ipAddress, ushort port, string listenAddress)
        {
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(ipAddress, port, listenAddress);
        }
    }
}