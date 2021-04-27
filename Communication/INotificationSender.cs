using System.Threading.Tasks;

using SteamGiveawaysBot.Server.Service.Models;

namespace SteamGiveawaysBot.Server.Communication
{
    public interface INotificationSender
    {
        Task SendNotificationAsync(Reward reward);
    }
}
