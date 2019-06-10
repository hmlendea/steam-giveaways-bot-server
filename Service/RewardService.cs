using System;
using System.Net;
using System.Net.Mail;
using System.Security.Authentication;

using NuciDAL.Repositories;
using NuciLog.Core;
using NuciSecurity.HMAC;

using SteamGiveawaysBot.Server.Api.Models;
using SteamGiveawaysBot.Server.Communication;
using SteamGiveawaysBot.Server.Configuration;
using SteamGiveawaysBot.Server.DataAccess.DataObjects;
using SteamGiveawaysBot.Server.Logging;
using SteamGiveawaysBot.Server.Security;
using SteamGiveawaysBot.Server.Service.Mapping;
using SteamGiveawaysBot.Server.Service.Models;

namespace SteamGiveawaysBot.Server.Service
{
    public class RewardService : IRewardService
    {
        readonly IMailSender mailSender;

        readonly IXmlRepository<UserEntity> userRepository;
        readonly IXmlRepository<RewardEntity> rewardRepository;

        readonly IHmacEncoder<RecordRewardRequest> requestHmacEncoder;
        readonly IHmacEncoder<RecordRewardRequest> responseHmacEncoder;

        readonly MailSettings mailSettings;

        readonly ILogger logger;

        public RewardService(
            IMailSender mailSender,
            IXmlRepository<UserEntity> userRepository,
            IXmlRepository<RewardEntity> rewardRepository,
            IHmacEncoder<RecordRewardRequest> requestHmacEncoder,
            IHmacEncoder<RecordRewardRequest> responseHmacEncoder,
            MailSettings mailSettings,
            ILogger logger)
        {
            this.mailSender = mailSender;
            this.userRepository = userRepository;
            this.rewardRepository = rewardRepository;
            this.requestHmacEncoder = requestHmacEncoder;
            this.responseHmacEncoder = responseHmacEncoder;
            this.mailSettings = mailSettings;
            this.logger = logger;
        }

        public void RecordReward(RecordRewardRequest request)
        {
            logger.Info(
                MyOperation.RecordReward,
                OperationStatus.Started,
                new LogInfo(MyLogInfoKey.User, request.Username));

            User user = userRepository.Get(request.Username).ToServiceModel();

            ValidateRequest(request, user);

            Reward reward = GetRewardObjectFromRequest(request);

            StoreReward(reward);
            SendMailNotification(reward);

            logger.Info(
                MyOperation.RecordReward,
                OperationStatus.Success,
                new LogInfo(MyLogInfoKey.User, request.Username));
        }
        
        void ValidateRequest(RecordRewardRequest request, User user)
        {
            bool isTokenValid = requestHmacEncoder.IsTokenValid(request.HmacToken, request, user.SharedSecretKey);

            if (!isTokenValid)
            {
                AuthenticationException ex = new AuthenticationException("The provided HMAC token is not valid");

                logger.Error(
                    MyOperation.RecordReward,
                    OperationStatus.Failure,
                    ex,
                    new LogInfo(MyLogInfoKey.User, request.Username));

                throw ex;
            }
        }

        Reward GetRewardObjectFromRequest(RecordRewardRequest request)
        {
            Reward reward = new Reward();
            reward.Id = $"{request.GiveawaysProvider}-{request.GiveawayId}-{reward.SteamUsername}";
            reward.GiveawaysProvider = request.GiveawaysProvider;
            reward.GiveawayId = request.GiveawayId;
            reward.SteamUsername = request.SteamUsername;
            reward.SteamAppId = request.SteamAppId;
            reward.ActivationKey = request.ActivationKey;

            return reward;
        }

        void StoreReward(Reward reward)
        {
            rewardRepository.Add(reward.ToDataObject());
            rewardRepository.ApplyChanges();
        }

        void SendMailNotification(Reward reward)
        {
            string subject = $"SGB: Key won";
            string body =
                $"Giveaway provider: {reward.GiveawaysProvider}{Environment.NewLine}" +
                $"Giveaway ID: {reward.GiveawayId}{Environment.NewLine}" +
                $"Store URL: {reward.SteamAppUrl}{Environment.NewLine}" +
                $"User: {reward.SteamUsername}{Environment.NewLine}" +
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
