namespace SteamAccountDistributor.DataAccess.DataObjects
{
    public sealed class UserEntity
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string AssignedSteamAccount { get; set; }
    }
}
