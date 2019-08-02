using System.Text.RegularExpressions;

namespace SteamGiveawaysBot.Server.Service.Models
{
    public sealed class Reward
    {
        const string SteamKeyRegexPattern = "^([A-Z0-9]{5}-)*[A-Z0-9]{5}$";

        const string SteamActivationUrlFormat = "https://store.steampowered.com/account/registerkey?key={0}";

        public string Id { get; set; }
        
        public string GiveawaysProvider { get; set; }

        public string GiveawayId { get; set; }

        public string SteamUsername { get; set; }

        public SteamApp SteamApp { get; set; }

        public string ActivationKey { get; set; }

        public bool IsKeySteamCode => Regex.IsMatch(ActivationKey, SteamKeyRegexPattern);

        public string ActivationLink
        {
            get
            {
                if (IsKeySteamCode)
                {
                    return string.Format(SteamActivationUrlFormat, ActivationKey);
                }
                
                if (ActivationKey.Contains("http"))
                {
                    return ActivationKey;
                }

                return "UNKNOWN";
            }
        }
    }
}
