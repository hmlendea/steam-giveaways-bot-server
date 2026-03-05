using NuciAPI.Responses;
using NuciSecurity.HMAC;

namespace SteamGiveawaysBot.Server.Api.Models
{
    public sealed class SteamAccountResponse : NuciApiSuccessResponse
    {
        [HmacOrder(1)]
        public string Username { get; set; }

        [HmacOrder(2)]
        public string Password { get; set; }
    }
}
