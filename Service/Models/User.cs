using System;

namespace SteamGiveawaysBot.Server.Service.Models
{
    public sealed class User
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public string SharedSecretKey { get; set; }

        public string AssignedSteamAccount { get; set; }

        public string IpAddress { get; set; }

        public DateTime CreationTime { get; set; }

        public DateTime LastUpdateTime { get; set; }

        public User()
        {
            CreationTime = DateTime.Now;
            LastUpdateTime = CreationTime;
        }
    }
}
