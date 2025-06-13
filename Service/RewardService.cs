using System.Security.Authentication;

using NuciDAL.Repositories;
using NuciLog.Core;
using NuciSecurity.HMAC;
using SteamGiveawaysBot.Server.Api.Models;
using SteamGiveawaysBot.Server.Client;
using SteamGiveawaysBot.Server.Communication;
using SteamGiveawaysBot.Server.DataAccess.DataObjects;
using SteamGiveawaysBot.Server.Logging;
using SteamGiveawaysBot.Server.Service.Mapping;
using SteamGiveawaysBot.Server.Service.Models;

namespace SteamGiveawaysBot.Server.Service
{
    public class RewardService(
        INotificationSender notificationSender,
        IRepository<UserEntity> userRepository,
        IRepository<RewardEntity> rewardRepository,
        IStorefrontDataRetriever storefrontDataRetriever,
        ILogger logger) : IRewardService
    {
        readonly INotificationSender notificationSender = notificationSender;

        readonly IRepository<UserEntity> userRepository = userRepository;
        readonly IRepository<RewardEntity> rewardRepository = rewardRepository;

        readonly IStorefrontDataRetriever storefrontDataRetriever = storefrontDataRetriever;
        readonly ILogger logger = logger;

        public void RecordReward(RecordRewardRequest request)
        {
            logger.Info(
                MyOperation.RecordReward,
                OperationStatus.Started,
                new LogInfo(MyLogInfoKey.User, request.Username),
                new LogInfo(MyLogInfoKey.GiveawaysProvider, request.GiveawaysProvider),
                new LogInfo(MyLogInfoKey.GiveawayId, request.GiveawayId));

            ValidateRequest(request);

            Reward reward = GetRewardObjectFromRequest(request);
            reward.SteamApp = storefrontDataRetriever.GetAppData(reward.SteamApp.Id).ToServiceModel();

            if (RewardWasAlreadyRecorded(reward))
            {
                logger.Warn(
                    MyOperation.RecordReward,
                    OperationStatus.Failure,
                    "Reward already recorded",
                    new LogInfo(MyLogInfoKey.User, request.Username),
                    new LogInfo(MyLogInfoKey.GiveawaysProvider, request.GiveawaysProvider),
                    new LogInfo(MyLogInfoKey.GiveawayId, request.GiveawayId));

                return;
            }

            StoreReward(reward);
            notificationSender.SendNotificationAsync(reward);

            logger.Info(
                MyOperation.RecordReward,
                OperationStatus.Success,
                new LogInfo(MyLogInfoKey.User, request.Username),
                new LogInfo(MyLogInfoKey.GiveawaysProvider, request.GiveawaysProvider),
                new LogInfo(MyLogInfoKey.GiveawayId, request.GiveawayId));
        }

        void ValidateRequest(RecordRewardRequest request)
        {
            User user = userRepository.TryGet(request.Username)?.ToServiceModel();

            if (user is null)
            {
                AuthenticationException ex = new("The provided user is not registered");

                logger.Error(
                    MyOperation.RecordReward,
                    OperationStatus.Failure,
                    ex,
                    new LogInfo(MyLogInfoKey.User, request.Username));

                throw ex;
            }

            bool isTokenValid = HmacEncoder.IsTokenValid(request.HmacToken, request, user.SharedSecretKey);

            if (!isTokenValid)
            {
                AuthenticationException ex = new("The provided HMAC token is not valid");

                logger.Error(
                    MyOperation.RecordReward,
                    OperationStatus.Failure,
                    ex,
                    new LogInfo(MyLogInfoKey.User, request.Username),
                    new LogInfo(MyLogInfoKey.GiveawaysProvider, request.GiveawaysProvider),
                    new LogInfo(MyLogInfoKey.GiveawayId, request.GiveawayId));

                throw ex;
            }
        }

        static Reward GetRewardObjectFromRequest(RecordRewardRequest request) => new()
        {
            Id = $"{request.GiveawaysProvider}-{request.GiveawayId}-{request.SteamUsername}",
            GiveawaysProvider = request.GiveawaysProvider,
            GiveawayId = request.GiveawayId,
            SteamUsername = request.SteamUsername,
            ActivationKey = request.ActivationKey,
            SteamApp = new SteamApp
            {
                Id = request.SteamAppId
            }
        };

        bool RewardWasAlreadyRecorded(Reward reward)
            => rewardRepository.TryGet(reward.Id) is not null;

        void StoreReward(Reward reward)
        {
            rewardRepository.Add(reward.ToDataObject());
            rewardRepository.ApplyChanges();
        }
    }
}
