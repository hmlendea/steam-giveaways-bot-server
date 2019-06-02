namespace SteamGiveawaysBot.Server.Service.Models
{
    public sealed class SteamAccount
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public bool IsSteamGiftsSuspended { get; set; }
    }
}
