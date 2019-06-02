using System.Xml.Serialization;

namespace SteamGiveawaysBot.Server.Service.Models
{
    public sealed class Reward
    {
        public string GiveawaysProvider { get; set; }

        public string GiveawayId { get; set; }

        public string SteamUsername { get; set; }

        public string SteamAppId { get; set; }

        [XmlIgnore]
        public string SteamAppUrl => $"https://store.steampowered.com/app/{SteamAppId}";

        public string GameTitle { get; set; }

        public string ActivationKey { get; set; }
    }
}
