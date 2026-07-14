using System;

namespace SteamGiveawaysBot.Server.Service.Models
{
    public sealed class SteamAccount
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public bool IsSteamGiftsSuspended { get; set; }

        public DateTimeOffset CreationTime { get; set; }

        public DateTimeOffset LastUpdateTime { get; set; }

        public SteamAccount()
        {
            CreationTime = DateTimeOffset.Now;
            LastUpdateTime = CreationTime;
        }
    }
}
