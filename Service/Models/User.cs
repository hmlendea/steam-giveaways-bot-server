namespace SteamAccountDistributor.Service.Models
{
    public sealed class User
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string AssignedSteamAccount { get; set; }
    }
}
