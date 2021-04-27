using System.Threading.Tasks;

namespace SteamGiveawaysBot.Server.Communication
{
    public interface INotificationSender
    {
        Task SendNotification(string content);
    }
}
