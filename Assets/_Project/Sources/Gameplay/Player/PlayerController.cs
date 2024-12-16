using GOBA.CORE;
using Unity.Netcode;

namespace GOBA
{
    public class PlayerController : GameEntity, IPlayerController
    {
        private int _playerId;
        private NetworkVariable<int> _playerTeam = new NetworkVariable<int>();

        public int GetPlayerId()
        {
            return _playerId;
        }

        public void SetPlayerId(int id)
        {
            _playerId = id;
        }

        public int GetPlayerTeam()
        {
            return _playerTeam.Value;
        }

        public void SetPlayerTeam(int team)
        {
            _playerTeam.Value = team;
        }
    }
}