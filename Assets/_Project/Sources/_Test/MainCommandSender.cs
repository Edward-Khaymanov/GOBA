using MapModCore;
using Unity.Netcode;
using UnityEngine;

namespace GOBA.Assets._Project.Sources._Test
{
    public interface ICommandSender
    {
        public void MoveTo(IUnit target, Vector3 position);
    }

    public class MainCommandSender : NetworkBehaviour, ICommandSender
    {
        public static MainCommandSender Instance { get; private set; }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            Instance = this;
        }

        public void MoveTo(IUnit target, Vector3 position)
        {
            MoveToRPC(target.NetworkBehaviour, position);
        }

        public void UseAbility(IUnit unitReference, int abilityId, AbilityCastData castData)
        {
            UseAbilityRPC(unitReference.NetworkBehaviour, abilityId, castData);
        }

        [Rpc(SendTo.Server, RequireOwnership = false)]
        public void MoveToRPC(NetworkBehaviourReference unitReference, Vector3 position, RpcParams rpcParams = default)
        {
            var clientId = rpcParams.Receive.SenderClientId;
            unitReference.TryGet<Unit>(out var unit);
            //if can controll
            var command = new MoveCommand(unit, position);
            command.Execute();
        }

        [Rpc(SendTo.Server, RequireOwnership = false)]
        public void UseAbilityRPC(NetworkBehaviourReference unitReference, int abilityId, AbilityCastData castData, RpcParams rpcParams = default)
        {
            var clientId = rpcParams.Receive.SenderClientId;
            unitReference.TryGet<Unit>(out var unit);
            //if can controll
            var command = new UseAbilityCommand(unit, abilityId, castData);
            command.Execute();
        }
    }
}