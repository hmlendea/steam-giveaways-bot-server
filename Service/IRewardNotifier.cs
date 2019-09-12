using System.Threading.Tasks;

using SteamGiveawaysBot.Server.Service.Models;

namespace SteamGiveawaysBot.Server.Service
{
    public interface IRewardNotifier
    {
        Task SendNotificationAsync(Reward reward);
    }
}
