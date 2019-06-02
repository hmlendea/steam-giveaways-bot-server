using System;
using System.Net;
using System.Net.Mail;
using System.Security.Authentication;

using SteamGiveawaysBot.Server.Api.Models;
using SteamGiveawaysBot.Server.Core.Configuration;
using SteamGiveawaysBot.Server.DataAccess.DataObjects;
using SteamGiveawaysBot.Server.DataAccess.Repositories;
using SteamGiveawaysBot.Server.Security;
using SteamGiveawaysBot.Server.Service.Mapping;
using SteamGiveawaysBot.Server.Service.Models;

namespace SteamGiveawaysBot.Server.Service
{
    public class RewardService : IRewardService
    {
        readonly IUserRepository userRepository;

        readonly IRewardRepository rewardRepository;

        readonly IHmacEncoder<RecordRewardRequest> requestHmacEncoder;

        readonly IHmacEncoder<RecordRewardRequest> responseHmacEncoder;

        readonly MailSettings mailSettings;

        public RewardService(

            IUserRepository userRepository,
            IRewardRepository rewardRepository,
            IHmacEncoder<RecordRewardRequest> requestHmacEncoder,
            IHmacEncoder<RecordRewardRequest> responseHmacEncoder,
            MailSettings mailSettings)
        {
            this.userRepository = userRepository;
            this.rewardRepository = rewardRepository;
            this.requestHmacEncoder = requestHmacEncoder;
            this.responseHmacEncoder = responseHmacEncoder;
            this.mailSettings = mailSettings;
        }

        public void RecordReward(RecordRewardRequest request)
        {
            User user = userRepository.Get(request.Username).ToServiceModel();

            //ValidateRequest(request, user);

            Reward reward = new Reward();
            reward.GiveawaysProvider = request.GiveawaysProvider;
            reward.GiveawayId = request.GiveawayId;
            reward.SteamUsername = request.SteamUsername;
            reward.SteamAppId = request.SteamAppId;
            reward.GameTitle = request.GameTitle;
            reward.ActivationKey = request.ActivationKey;

            rewardRepository.Add(reward.ToDataObject());

            SendMailNotification(reward);
        }
        
        void ValidateRequest(RecordRewardRequest request, User user)
        {
            bool isTokenValid = requestHmacEncoder.IsTokenValid(request.HmacToken, request, user.SharedSecretKey);

            if (!isTokenValid)
            {
                throw new AuthenticationException("The provided HMAC token is not valid");
            }
        }

        void SendMailNotification(Reward reward)
        {
            MailAddress senderAddress = new MailAddress(mailSettings.SenderAddress, "SteamGiveawaysBot");
            NetworkCredential senderCredentials = new NetworkCredential(mailSettings.SenderAddress, mailSettings.SenderPassword);
            SmtpClient client = new SmtpClient();
            client.Host = "smtp.gmail.com";
            client.Port = 587;
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Credentials = senderCredentials;

            using (MailMessage mail = new MailMessage())
            {
                mail.From = senderAddress;
                mail.Subject = $"SGB: \"{reward.GameTitle}\" key won";
                mail.Body =
                    $"Game title: {reward.GameTitle}{Environment.NewLine}" +
                    $"Giveaway provider: {reward.GiveawaysProvider}{Environment.NewLine}" +
                    $"Giveaway ID: {reward.GiveawayId}{Environment.NewLine}" +
                    $"Store URL: {reward.SteamAppUrl}{Environment.NewLine}" +
                    $"User: {reward.SteamUsername}{Environment.NewLine}" +
                    $"Activation key: {reward.ActivationKey}";
                mail.To.Add(mailSettings.RecipientAddress);

                client.Send(mail);
            }
        }
    }
}
