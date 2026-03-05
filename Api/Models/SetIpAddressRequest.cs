using System.Text.Json.Serialization;
using NuciAPI.Requests;

namespace SteamGiveawaysBot.Server.Api.Models
{
    public sealed class SetIpAddressRequest : NuciApiRequest
    {
        public string Username { get; set; }

        [JsonPropertyName("ip")]
        public string IpAddress { get; set; }
    }
}
