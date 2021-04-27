using System;
using System.Threading.Tasks;

using SteamGiveawaysBot.Server.Communication;
using SteamGiveawaysBot.Server.Service.Models;

namespace SteamGiveawaysBot.Server.Service
{
    public sealed class RewardNotifier : IRewardNotifier
    {
        readonly INotificationSender notificationSender;

        public RewardNotifier(INotificationSender notificationSender)
        {
            this.notificationSender = notificationSender;
        }

        public async Task SendNotificationAsync(Reward reward)
        {
            string subject = $"SGB: \"({reward.SteamApp.Name})\" key won";
            string body =
                $"User: {reward.SteamUsername}{Environment.NewLine}" +
                $"App name: {reward.SteamApp.Name}{Environment.NewLine}" +
                $"Giveaway URL: {reward.GiveawayUrl}{Environment.NewLine}" +
                $"Store URL: {reward.SteamApp.StoreUrl}{Environment.NewLine}" +
                $"Activation link: {reward.ActivationLink}{Environment.NewLine}" +
                $"Activation key: {reward.ActivationKey}";

            await notificationSender.SendNotification($"{subject}\n{body}");
        }
    }
}
