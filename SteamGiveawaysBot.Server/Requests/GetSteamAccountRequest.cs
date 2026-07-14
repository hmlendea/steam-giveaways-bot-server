using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using NuciAPI.Requests;
using NuciSecurity.HMAC;

namespace SteamGiveawaysBot.Server.Requests
{
    public sealed class GetSteamAccountRequest : NuciApiRequest
    {
        [HmacOrder(1)]
        [JsonPropertyName("username")]
        [FromQuery(Name = "username")]
        public string Username { get; set; }

        [HmacOrder(2)]
        [JsonPropertyName("gaProvider")]
        [FromQuery(Name = "gaProvider")]
        public string GiveawaysProvider { get; set; }
    }
}
