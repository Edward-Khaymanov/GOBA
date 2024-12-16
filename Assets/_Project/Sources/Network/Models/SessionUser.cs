namespace GOBA.Network
{
    public class SessionUser
    {
        public UserID UserId { get; set; }
        public NetworkUser NetworkUser { get; set; }
        public UserState State { get; set; }
        public SessionTeam Team { get; set; }

        public bool IsValid => NetworkUser.Equals(default(NetworkUser)) == false && Team != null;
    }

    public struct UserID
    {
        public UserID(ulong id)
        {
            Id = id;
        }

        public ulong Id;
    }
}