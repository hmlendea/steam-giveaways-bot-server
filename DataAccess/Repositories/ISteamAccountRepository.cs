using System.Collections.Generic;

using SteamGiveawaysBot.Server.DataAccess.DataObjects;

namespace SteamGiveawaysBot.Server.DataAccess.Repositories
{
    public interface ISteamAccountRepository
    {
        IEnumerable<SteamAccountEntity> GetAll();

        SteamAccountEntity Get(string username);
    }
}
