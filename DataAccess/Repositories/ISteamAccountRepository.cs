using System.Collections.Generic;

using SteamAccountDistributor.DataAccess.DataObjects;

namespace SteamAccountDistributor.DataAccess.Repositories
{
    public interface ISteamAccountRepository
    {
        IEnumerable<SteamAccountEntity> GetAll();

        SteamAccountEntity Get(string username);
    }
}
