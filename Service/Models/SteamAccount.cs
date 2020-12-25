using System;

namespace SteamGiveawaysBot.Server.Service.Models
{
    public sealed class SteamAccount
    {
        public string Id { get; set; }
        
        public string Username { get; set; }

        public string Password { get; set; }

        public bool IsSteamGiftsSuspended { get; set; }

        public DateTime CreationTime { get; set; }

        public DateTime LastUpdateTime { get; set; }

        public SteamAccount()
        {
            CreationTime = DateTime.Now;
            LastUpdateTime = CreationTime;
        }
    }
}
