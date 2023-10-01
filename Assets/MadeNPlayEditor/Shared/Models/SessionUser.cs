namespace MadeNPlayShared
{
    public class SessionUser
    {
        public ulong UserId { get; set; }
        public NetworkUser NetworkUser { get; set; }
        public UserState State { get; set; }
        public SessionTeam Team { get; set; }

        public bool IsValid => NetworkUser.Equals(default(NetworkUser)) == false && Team != null;
    }
}