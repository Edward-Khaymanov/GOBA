using System.Collections.Generic;

namespace GOBA.Network
{
    public class SessionTeam
    {
        public SessionTeam(int id, int maxUsers)
        {
            Id = id;
            MaxUsers = maxUsers;
            Users = new List<SessionUser>();
        }

        public int Id { get; private set; }
        public int MaxUsers { get; private set; }
        public List<SessionUser> Users { get; private set; }
        public int UsersCount => Users.Count;
        public bool CanJoin => MaxUsers >= UsersCount + 1;
    }
}