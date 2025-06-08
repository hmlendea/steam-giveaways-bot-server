using System.Threading.Tasks;

using Telegram.Bot;
using Telegram.Bot.Types.Enums;

using SteamGiveawaysBot.Server.Configuration;
using SteamGiveawaysBot.Server.Service.Models;

namespace SteamGiveawaysBot.Server.Communication
{
    public class TelegramNotificationSender(TelegramSettings telegramSettings) : INotificationSender
    {
        readonly TelegramSettings telegramSettings = telegramSettings;

        readonly TelegramBotClient botClient = new(telegramSettings.AccessToken);

        public async Task SendNotificationAsync(Reward reward)
            => await botClient.SendMessage(
                chatId: telegramSettings.ChatId,
                parseMode: ParseMode.Markdown,
                text:
                    $"*Key won*: [{reward.SteamApp.Name}]({reward.SteamApp.StoreUrl}) " +
                    $"_(by _[{reward.SteamUsername}]({reward.GiveawayUrl}/winners)_)_\n" +
                    $"[{reward.ActivationKey}]({reward.ActivationLink})"
            );
    }
}
