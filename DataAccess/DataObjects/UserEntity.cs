using NuciDAL.DataObjects;

namespace SteamGiveawaysBot.Server.DataAccess.DataObjects
{
    public sealed class UserEntity : EntityBase
    {
        public string Username { get; set; }

        public string SharedSecretKey { get; set; }

        public string AssignedSteamAccount { get; set; }
    }
}
