using System;
using System.Security.Authentication;

using NuciDAL.Repositories;
using NuciSecurity.HMAC;
using SteamGiveawaysBot.Server.Api.Models;
using SteamGiveawaysBot.Server.DataAccess.DataObjects;
using SteamGiveawaysBot.Server.Service.Mapping;
using SteamGiveawaysBot.Server.Service.Models;

namespace SteamGiveawaysBot.Server.Service
{
    public sealed class UserService(IRepository<UserEntity> userRepository) : IUserService
    {
        public void SetIpAddress(SetIpAddressRequest request)
        {
            User user = userRepository.Get(request.Username).ToServiceModel();

            ValidateRequest(request, user);

            user.IpAddress = request.IpAddress;
            user.LastUpdateTime = DateTime.Now;

            userRepository.Update(user.ToDataObject());
            userRepository.ApplyChanges();
        }

        static void ValidateRequest(SetIpAddressRequest request, User user)
        {
            if (!HmacEncoder.IsTokenValid(request.HmacToken, request, user.SharedSecretKey))
            {
                throw new AuthenticationException("The provided HMAC token is not valid");
            }
        }
    }
}
