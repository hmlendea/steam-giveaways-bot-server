using System.Text.Json.Serialization;
using NuciAPI.Requests;

namespace SteamGiveawaysBot.Server.Api.Models
{
    public sealed class SteamAccountRequest : Request
    {
        public string Username { get; set; }

        [JsonPropertyName("gaProvider")]
        public string GiveawaysProvider { get; set; }
    }
}
