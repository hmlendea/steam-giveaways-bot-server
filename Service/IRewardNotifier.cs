using SteamGiveawaysBot.Server.Service.Models;

namespace SteamGiveawaysBot.Server.Service
{
    public interface IRewardNotifier
    {
        void SendNotification(Reward reward);
    }
}
