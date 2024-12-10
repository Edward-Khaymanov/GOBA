using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

namespace GOBA
{
    public static class HELPERS
    {
        public static ClientRpcParams GetClientRPC(ulong clientId)
        {
            var target = new ClientRpcParams()
            {
                Send = new ClientRpcSendParams()
                {
                    TargetClientIds = new ulong[] { clientId }
                }
            };

            return target;
        }

        public static ClientRpcParams GetClientsRPC(IEnumerable<ulong> clientsId)
        {
            var target = new ClientRpcParams()
            {
                Send = new ClientRpcSendParams()
                {
                    TargetClientIds = clientsId.ToList()
                }
            };

            return target;
        }

        public static ClientRpcParams GetObserverRPC()
        {
            var target = new ClientRpcParams()
            {
                Send = new ClientRpcSendParams()
                {
                    TargetClientIds = NetworkManager.Singleton.ConnectedClientsIds
                }
            };

            return target;
        }

        
    }
}