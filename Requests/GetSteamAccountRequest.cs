using System.Text.Json.Serialization;
using NuciAPI.Requests;
using NuciSecurity.HMAC;

namespace SteamGiveawaysBot.Server.Requests
{
    public sealed class GetSteamAccountRequest : NuciApiRequest
    {
        [HmacOrder(1)]
        public string Username { get; set; }

        [HmacOrder(2)]
        [JsonPropertyName("gaProvider")]
        public string GiveawaysProvider { get; set; }
    }
}
