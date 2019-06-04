namespace SteamGiveawaysBot.Server.Service.Models
{
    public sealed class User
    {
        public string Id { get; set; }
        
        public string Username { get; set; }

        public string SharedSecretKey { get; set; }

        public string AssignedSteamAccount { get; set; }
    }
}
