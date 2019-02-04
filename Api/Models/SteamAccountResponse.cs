namespace SteamAccountDistributor.Api.Models
{
    public sealed class SteamAccountResponse : SuccessResponse
    {
        public string Username { get; set; }

        public string Password { get; set; }
    }
}
