using System.Xml.Serialization;

namespace SteamGiveawaysBot.Server.Service.Models
{
    public sealed class SteamApp
    {
        public string Id { get; set; }

        public string Name { get; set; }

        [XmlIgnore]
        public string StoreUrl => $"https://store.steampowered.com/app/{Id}";
    }
}
