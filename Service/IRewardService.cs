using SteamGiveawaysBot.Server.Api.Models;

namespace SteamGiveawaysBot.Server.Service
{
    public interface IRewardService
    {
        void RecordReward(RecordRewardRequest request);
    }
}
