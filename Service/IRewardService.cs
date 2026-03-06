using SteamGiveawaysBot.Server.Requests;

namespace SteamGiveawaysBot.Server.Service
{
    public interface IRewardService
    {
        void RecordReward(RecordRewardRequest request);
    }
}
