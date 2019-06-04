using System;

using NuciDAL.Repositories;

using SteamGiveawaysBot.Server.Configuration;
using SteamGiveawaysBot.Server.DataAccess.DataObjects;

namespace SteamGiveawaysBot.Server.DataAccess.Repositories
{
    public sealed class SteamAccountRepository : XmlRepository<SteamAccountEntity>, ISteamAccountRepository
    {
        public SteamAccountRepository(ApplicationSettings settings)
            : base(settings.SteamAccountStorePath)
        {
        }
        
        public override void Update(SteamAccountEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
