using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

using Telegram.Bot;

using SteamGiveawaysBot.Server.Configuration;

namespace SteamGiveawaysBot.Server.Communication
{
    public class TelegramNotificationSender : INotificationSender
    {
        readonly TelegramSettings telegramSettings;
        
        readonly ITelegramBotClient botClient;

        public TelegramNotificationSender(TelegramSettings telegramSettings)
        {
            this.telegramSettings = telegramSettings;

            botClient = new TelegramBotClient(telegramSettings.AccessToken);
        }

        public async Task SendNotification(string content)
        {
            await botClient.SendTextMessageAsync(
                chatId: telegramSettings.ChatId,
                text: content
            );
        }
    }
}
