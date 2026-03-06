using NuciAPI.Responses;
using NuciSecurity.HMAC;

namespace SteamGiveawaysBot.Server.Responses
{
    public sealed class GetSteamAccountResponse : NuciApiSuccessResponse
    {
        [HmacOrder(1)]
        public string Username { get; set; }

        [HmacOrder(2)]
        public string Password { get; set; }
    }
}
