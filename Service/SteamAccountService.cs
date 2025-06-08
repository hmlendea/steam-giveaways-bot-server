using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;

using NuciDAL.Repositories;
using NuciExtensions;
using NuciSecurity.HMAC;

using SteamGiveawaysBot.Server.Api.Models;
using SteamGiveawaysBot.Server.DataAccess.DataObjects;
using SteamGiveawaysBot.Server.Service.Mapping;
using SteamGiveawaysBot.Server.Service.Models;

namespace SteamGiveawaysBot.Server.Service
{
    public sealed class SteamAccountService(
        IRepository<UserEntity> userRepository,
        IRepository<SteamAccountEntity> steamAccountRepository,
        IHmacEncoder<SteamAccountRequest> requestHmacEncoder,
        IHmacEncoder<SteamAccountResponse> responseHmacEncoder) : ISteamAccountService
    {
        readonly IRepository<UserEntity> userRepository = userRepository;
        readonly IRepository<SteamAccountEntity> steamAccountRepository = steamAccountRepository;

        readonly IHmacEncoder<SteamAccountRequest> requestHmacEncoder = requestHmacEncoder;
        readonly IHmacEncoder<SteamAccountResponse> responseHmacEncoder = responseHmacEncoder;

        public SteamAccountResponse GetAccount(SteamAccountRequest request)
        {
            User user = userRepository.Get(request.Username).ToServiceModel();

            ValidateRequest(request, user);

            SteamAccount assignedAccount = GetAssignedAccount(user, request.GiveawaysProvider);
            SteamAccountResponse response = CreateResponse(user, assignedAccount);

            return response;
        }

        void ValidateRequest(SteamAccountRequest request, User user)
        {
            if (!requestHmacEncoder.IsTokenValid(request.HmacToken, request, user.SharedSecretKey))
            {
                throw new AuthenticationException("The provided HMAC token is not valid");
            }
        }

        SteamAccount GetAssignedAccount(User user, string gaProvider)
        {
            bool needsReuser = DoesItNeedReuser(user, gaProvider);
            SteamAccount assignedAccount;

            if (needsReuser)
            {
                assignedAccount = FindAccountToAssign(gaProvider);

                user.AssignedSteamAccount = assignedAccount.Username;
                userRepository.Update(user.ToDataObject());
            }
            else
            {
                assignedAccount = steamAccountRepository.Get(user.AssignedSteamAccount).ToServiceModel();
            }

            return assignedAccount;
        }

        SteamAccountResponse CreateResponse(User user, SteamAccount steamAccount)
        {
            SteamAccountResponse response = new()
            {
                Username = steamAccount.Username,
                Password = steamAccount.Password
            };

            response.HmacToken = responseHmacEncoder.GenerateToken(response, user.SharedSecretKey);

            return response;
        }

        SteamAccount FindAccountToAssign(string gaProvider)
        {
            IEnumerable<User> users = userRepository.GetAll().ToServiceModels();
            IEnumerable<SteamAccount> steamAccounts = steamAccountRepository.GetAll().ToServiceModels();

            steamAccounts = steamAccounts.Where(x => users.All(y => y.AssignedSteamAccount != x.Username));

            if (gaProvider.Equals("SteamGifts", StringComparison.InvariantCultureIgnoreCase))
            {
                steamAccounts = steamAccounts.Where(x => !x.IsSteamGiftsSuspended);
            }

            return steamAccounts.GetRandomElement();
        }

        bool DoesItNeedReuser(User user, string gaProvider)
        {
            if (string.IsNullOrWhiteSpace(user.AssignedSteamAccount))
            {
                return true;
            }

            SteamAccount account = steamAccountRepository.Get(user.AssignedSteamAccount).ToServiceModel();

            if (gaProvider.Equals("SteamGifts", StringComparison.InvariantCultureIgnoreCase) &&
                account.IsSteamGiftsSuspended)
            {
                return true;
            }

            return false;
        }
    }
}
