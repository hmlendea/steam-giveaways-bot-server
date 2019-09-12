using NuciDAL.DataObjects;

namespace SteamGiveawaysBot.Server.DataAccess.DataObjects
{
    public sealed class RewardEntity : EntityBase
    {
        public string GiveawaysProvider { get; set; }

        public string SteamUsername { get; set; }

        public string GiveawayId { get; set; }

        public string SteamAppId { get; set; }

        public string SteamAppUrl => $"https://store.steampowered.com/app/{SteamAppId}";

        public string GameTitle { get; set; }

        public string ActivationKey { get; set; }
    }
}
