namespace SteamGiveawaysBot.Server.Api.Models
{
    public sealed class SteamAccountRequest : Request
    {
        public string Username { get; set; }

        public string GiveawaysProvider { get; set; }
    }
}
