namespace SteamAccountDistributor.Api.Models
{
    public sealed class SteamAccountRequest
    {
        public string Hostname { get; set; }

        public string Password { get; set; }

        public AccountStatus AccountStatus { get; set; }
    }
}
