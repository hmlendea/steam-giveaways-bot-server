using System.Text.Json.Serialization;
using NuciAPI.Requests;
using NuciSecurity.HMAC;

namespace SteamGiveawaysBot.Server.Api.Models
{
    public sealed class SteamAccountRequest : NuciApiRequest
    {
        [HmacOrder(1)]
        public string Username { get; set; }

        [HmacOrder(2)]
        [JsonPropertyName("gaProvider")]
        public string GiveawaysProvider { get; set; }
    }
}
