using System.Collections.Generic;

using SteamGiveawaysBot.Server.DataAccess.DataObjects;

namespace SteamGiveawaysBot.Server.DataAccess.Repositories
{
    public interface IUserRepository
    {
        IEnumerable<UserEntity> GetAll();

        UserEntity Get(string username);

        void Update(UserEntity user);
    }
}
