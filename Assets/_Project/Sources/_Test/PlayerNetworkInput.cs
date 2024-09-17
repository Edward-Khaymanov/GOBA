using System;
using Unity.Netcode;
using UnityEngine;

namespace GOBA
{
    public class PlayerNetworkInput : NetworkBehaviour
    {
        public NetworkVariable<Vector3> ViewPortMousePosition = new NetworkVariable<Vector3>(writePerm: NetworkVariableWritePermission.Owner);

        public event Action<Vector3> Moving;

        public override void OnNetworkSpawn()
        {
            NetworkManager.NetworkTickSystem.Tick += OnNetworkTick;
        }

        public override void OnNetworkDespawn()
        {
            NetworkManager.NetworkTickSystem.Tick += OnNetworkTick;
        }

        private void OnNetworkTick()
        {
            if (IsServer)
            {
                Debug.Log(ViewPortMousePosition.Value);
            }
        }

        [Rpc(SendTo.Server)]
        public void MoveRpc(Vector3 position)
        {
            Moving?.Invoke(position);
            Debug.Log($"MoveRpcTo: {position}");
        }
    }
}