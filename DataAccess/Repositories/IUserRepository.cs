using System.Collections.Generic;

using SteamAccountDistributor.DataAccess.DataObjects;

namespace SteamAccountDistributor.DataAccess.Repositories
{
    public interface IUserRepository
    {
        IEnumerable<UserEntity> GetAll();

        UserEntity Get(string username);
    }
}
