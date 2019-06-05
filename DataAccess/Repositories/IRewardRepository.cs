using System.Collections.Generic;

using NuciDAL.Repositories;

using SteamGiveawaysBot.Server.DataAccess.DataObjects;

namespace SteamGiveawaysBot.Server.DataAccess.Repositories
{
    public interface IRewardRepository : IXmlRepository<RewardEntity>
    {
        IEnumerable<RewardEntity> GetAll();

        void Add(RewardEntity entity);
    }
}
