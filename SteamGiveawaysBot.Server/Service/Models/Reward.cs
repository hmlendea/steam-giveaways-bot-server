using System;
using System.Text.RegularExpressions;

namespace SteamGiveawaysBot.Server.Service.Models
{
    public sealed class Reward
    {
        public string Id { get; set; }

        public string GiveawaysProvider { get; set; }

        public string GiveawayId { get; set; }

        public string GiveawayUrl
        {
            get
            {
                if (string.Equals(
                    GiveawaysProvider,
                    SteamGiftsProviderName,
                    StringComparison.InvariantCultureIgnoreCase))
                {
                    return $"{SteamGiftsGiveawayUrlBase}{GiveawayId}/ga/";
                }

                return $"[UNKNOWN] Provider={GiveawaysProvider}, Id={GiveawayId}";
            }
        }

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
                    return $"{SteamActivationUrlBase}{ActivationKey}";
                }

                if (ActivationKey.Contains(HttpSchemePrefix))
                {
                    return ActivationKey;
                }

                return UnknownActivationLink;
            }
        }

        public DateTimeOffset CreationTime { get; set; }

        public Reward() => CreationTime = DateTimeOffset.Now;

        private static string SteamKeyRegexPattern => "^([A-Z0-9]{5}-)*[A-Z0-9]{5}$";

        private static string SteamActivationUrlBase
            => "https://store.steampowered.com/account/registerkey?key=";

        private static string SteamGiftsProviderName => "SteamGifts";

        private static string SteamGiftsGiveawayUrlBase => "https://steamgifts.com/giveaway/";

        private static string HttpSchemePrefix => "http";

        private static string UnknownActivationLink => "UNKNOWN";
    }
}
