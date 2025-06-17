using System.Text.Json.Serialization;
using NuciAPI.Requests;

namespace SteamGiveawaysBot.Server.Api.Models
{
    public sealed class RecordRewardRequest : Request
    {
        public string Username { get; set; }

        [JsonPropertyName("gaProvider")]
        public string GiveawaysProvider { get; set; }

        [JsonPropertyName("gaId")]
        public string GiveawayId { get; set; }

        public string SteamUsername { get; set; }

        public string SteamAppId { get; set; }

        [JsonPropertyName("key")]
        public string ActivationKey { get; set; }
    }
}
