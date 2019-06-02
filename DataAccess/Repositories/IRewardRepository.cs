using System.Collections.Generic;

using SteamGiveawaysBot.Server.DataAccess.DataObjects;

namespace SteamGiveawaysBot.Server.DataAccess.Repositories
{
    public interface IRewardRepository
    {
        IEnumerable<RewardEntity> GetAll();

        void Add(RewardEntity entity);
    }
}
