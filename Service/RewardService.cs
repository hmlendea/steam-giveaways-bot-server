using System.Security.Authentication;

using SteamGiveawaysBot.Server.Api.Models;
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

        public RewardService(
            IUserRepository userRepository,
            IRewardRepository rewardRepository,
            IHmacEncoder<RecordRewardRequest> requestHmacEncoder,
            IHmacEncoder<RecordRewardRequest> responseHmacEncoder)
        {
            this.userRepository = userRepository;
            this.rewardRepository = rewardRepository;
            this.requestHmacEncoder = requestHmacEncoder;
            this.responseHmacEncoder = responseHmacEncoder;
        }

        public void RecordReward(RecordRewardRequest request)
        {
            User user = userRepository.Get(request.Username).ToServiceModel();

            //ValidateRequest(request, user);

            RewardEntity reward = new RewardEntity();
            reward.GiveawaysProvider = request.GiveawaysProvider;
            reward.GiveawayId = request.GiveawayId;
            reward.SteamUsername = request.SteamUsername;
            reward.SteamAppId = request.SteamAppId;
            reward.GameTitle = request.GameTitle;
            reward.ActivationKey = request.ActivationKey;

            rewardRepository.Add(reward);
        }
        
        void ValidateRequest(RecordRewardRequest request, User user)
        {
            bool isTokenValid = requestHmacEncoder.IsTokenValid(request.HmacToken, request, user.SharedSecretKey);

            if (!isTokenValid)
            {
                throw new AuthenticationException("The provided HMAC token is not valid");
            }
        }
    }
}
