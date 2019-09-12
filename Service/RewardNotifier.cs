using System;

using SteamGiveawaysBot.Server.Communication;
using SteamGiveawaysBot.Server.Configuration;
using SteamGiveawaysBot.Server.Service.Models;

namespace SteamGiveawaysBot.Server.Service
{
    public sealed class RewardNotifier : IRewardNotifier
    {
        readonly IMailSender mailSender;
        readonly MailSettings mailSettings;

        public RewardNotifier(
            IMailSender mailSender,
            MailSettings mailSettings)
        {
            this.mailSender = mailSender;
            this.mailSettings = mailSettings;
        }

        public void SendNotification(Reward reward)
        {
            string subject = $"SGB: \"({reward.SteamApp.Name})\" key won";
            string body =
                $"User: {reward.SteamUsername}{Environment.NewLine}" +
                $"App name: {reward.SteamApp.Name}{Environment.NewLine}" +
                $"Giveaway URL: {reward.GiveawayUrl}{Environment.NewLine}" +
                $"Store URL: {reward.SteamApp.StoreUrl}{Environment.NewLine}" +
                $"Activation link: {reward.ActivationLink}{Environment.NewLine}" +
                $"Activation key: {reward.ActivationKey}";

            mailSender.SendMail(
                mailSettings.SenderAddress,
                mailSettings.SenderName,
                mailSettings.SenderPassword,
                subject,
                body,
                mailSettings.RecipientAddress);
        }
    }
}
