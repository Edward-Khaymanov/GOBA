using System;
using System.Collections.Generic;

namespace MadeNPlayShared
{
    public class LobbyData
    {
        public LobbyData(Guid lobbyId, int maxPlayers, List<SessionUser> sessionUsers, List<SessionTeam> sessionTeams)
        {
            LobbyId = lobbyId;
            MaxPlayers = maxPlayers;
            SessionUsers = sessionUsers;
            SessionTeams = sessionTeams;
        }

        public Guid LobbyId { get; private set; }
        public int MaxPlayers { get; private set; }
        public LobbyState State { get; private set; }
        public List<SessionUser> SessionUsers { get; private set; }
        public List<SessionTeam> SessionTeams { get; private set; }

        public int CurrentPlayers => SessionUsers.Count;

        public void SetState(LobbyState state)
        {
            State = state;
        }
    }
}