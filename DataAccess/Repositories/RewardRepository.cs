using System;

using NuciDAL.Repositories;

using SteamGiveawaysBot.Server.Configuration;
using SteamGiveawaysBot.Server.DataAccess.DataObjects;

namespace SteamGiveawaysBot.Server.DataAccess.Repositories
{
    public sealed class RewardRepository : XmlRepository<RewardEntity>
    {
        public RewardRepository(DataStoreSettings settings)
            : base(settings.RewardsStorePath)
        {
        }

        public override void Update(RewardEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
