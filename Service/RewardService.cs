using System;
using System.Security.Authentication;

using NuciDAL.Repositories;
using NuciLog.Core;
using NuciSecurity.HMAC;

using SteamGiveawaysBot.Server.Api.Models;
using SteamGiveawaysBot.Server.Client;
using SteamGiveawaysBot.Server.DataAccess.DataObjects;
using SteamGiveawaysBot.Server.Logging;
using SteamGiveawaysBot.Server.Service.Mapping;
using SteamGiveawaysBot.Server.Service.Models;

namespace SteamGiveawaysBot.Server.Service
{
    public class RewardService : IRewardService
    {
        readonly IRewardNotifier rewardNotifier;

        readonly IRepository<UserEntity> userRepository;
        readonly IRepository<RewardEntity> rewardRepository;

        readonly IHmacEncoder<RecordRewardRequest> requestHmacEncoder;
        readonly IHmacEncoder<RecordRewardRequest> responseHmacEncoder;

        readonly IStorefrontDataRetriever storefrontDataRetriever;
        readonly ILogger logger;

        public RewardService(
            IRewardNotifier rewardNotifier,
            IRepository<UserEntity> userRepository,
            IRepository<RewardEntity> rewardRepository,
            IHmacEncoder<RecordRewardRequest> requestHmacEncoder,
            IHmacEncoder<RecordRewardRequest> responseHmacEncoder,
            IStorefrontDataRetriever storefrontDataRetriever,
            ILogger logger)
        {
            this.rewardNotifier = rewardNotifier;
            this.userRepository = userRepository;
            this.rewardRepository = rewardRepository;
            this.requestHmacEncoder = requestHmacEncoder;
            this.responseHmacEncoder = responseHmacEncoder;
            this.storefrontDataRetriever = storefrontDataRetriever;
            this.logger = logger;
        }

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

            if (WasRewardAlreadyRecorded(reward))
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
            rewardNotifier.SendNotification(reward);

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
                AuthenticationException ex = new AuthenticationException("The provided user is not registered");

                logger.Error( MyOperation.RecordReward, OperationStatus.Failure, ex, new LogInfo(MyLogInfoKey.User, request.Username));

                throw ex;
            }

            bool isTokenValid = requestHmacEncoder.IsTokenValid(request.HmacToken, request, user.SharedSecretKey);

            if (!isTokenValid)
            {
                AuthenticationException ex = new AuthenticationException("The provided HMAC token is not valid");

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

        Reward GetRewardObjectFromRequest(RecordRewardRequest request)
        {
            Reward reward = new Reward();
            reward.Id = $"{request.GiveawaysProvider}-{request.GiveawayId}-{reward.SteamUsername}";
            reward.GiveawaysProvider = request.GiveawaysProvider;
            reward.GiveawayId = request.GiveawayId;
            reward.SteamUsername = request.SteamUsername;
            reward.ActivationKey = request.ActivationKey;

            reward.SteamApp = new SteamApp();
            reward.SteamApp.Id = request.SteamAppId;
            
            return reward;
        }

        bool WasRewardAlreadyRecorded(Reward reward)
        {
            return rewardRepository.TryGet(reward.Id) != null;
        }

        void StoreReward(Reward reward)
        {
            rewardRepository.Add(reward.ToDataObject());
            rewardRepository.ApplyChanges();
        }
    }
}
