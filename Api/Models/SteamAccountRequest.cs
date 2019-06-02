namespace SteamGiveawaysBot.Server.Api.Models
{
    public sealed class SteamAccountRequest
    {
        public string Username { get; set; }

        public string GiveawaysProvider { get; set; }

        public string HmacToken { get; set; }
    }
}
