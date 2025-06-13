using NuciAPI.Requests;

namespace SteamGiveawaysBot.Server.Api.Models
{
    public sealed class SetIpAddressRequest : Request
    {
        public string Username { get; set; }

        public string IpAddress { get; set; }
    }
}
