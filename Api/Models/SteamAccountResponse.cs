using NuciAPI.Responses;

namespace SteamGiveawaysBot.Server.Api.Models
{
    public sealed class SteamAccountResponse : NuciApiSuccessResponse
    {
        public string Username { get; set; }

        public string Password { get; set; }
    }
}
