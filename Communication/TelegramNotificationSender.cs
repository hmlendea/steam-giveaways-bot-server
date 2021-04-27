using System.Threading.Tasks;

using Telegram.Bot;
using Telegram.Bot.Types.Enums;

using SteamGiveawaysBot.Server.Configuration;
using SteamGiveawaysBot.Server.Service.Models;

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

        public async Task SendNotificationAsync(Reward reward)
        {
            /*
            string message = $"You have won a key for \"**{reward.SteamApp.Name}**\"!";
            string body =
                $"User: {reward.SteamUsername}{Environment.NewLine}" +
                $"App name: {reward.SteamApp.Name}{Environment.NewLine}" +
                $"Giveaway URL: {reward.GiveawayUrl}{Environment.NewLine}" +
                $"Store URL: {reward.SteamApp.StoreUrl}{Environment.NewLine}" +
                $"Activation link: {reward.ActivationLink}{Environment.NewLine}" +
                $"Activation key: {reward.ActivationKey}";
            */

            await botClient.SendTextMessageAsync(
                chatId: telegramSettings.ChatId,
                parseMode: ParseMode.Markdown,
                text:
                    $"*Key won*: [{reward.SteamApp.Name}]({reward.SteamApp.StoreUrl}) " +
                    $"_(by _[{reward.SteamUsername}]({reward.GiveawayUrl})_)_\n" +
                    $"[{reward.ActivationKey}]({reward.ActivationLink})"
            );
        }
    }
}
