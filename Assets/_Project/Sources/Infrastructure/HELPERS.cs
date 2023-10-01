using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public static Vector2 GetXZ(this Vector3 vector)
        {
            return new Vector2(vector.x, vector.z);
        }

        public static float FlatDistanceTo(this Vector3 from, Vector3 to)
        {
            var a = from.GetXZ();
            var b = to.GetXZ();
            return Vector2.Distance(a, b);
        }
    }
}