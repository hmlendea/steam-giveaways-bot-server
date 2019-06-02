namespace SteamGiveawaysBot.Server.Api.Models
{
    public sealed class RecordRewardRequest
    {
        public string Username { get; set; }

        public string GiveawaysProvider { get; set; }

        public string GiveawayId { get; set; }

        public string SteamUsername { get; set; }

        public string SteamAppId { get; set; }

        public string GameTitle { get; set; }

        public string ActivationKey { get; set; }

        public string HmacToken { get; set; }
    }
}
