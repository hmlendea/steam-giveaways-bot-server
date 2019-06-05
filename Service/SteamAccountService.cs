using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;

using NuciDAL.Repositories;
using NuciExtensions;
using NuciSecurity.HMAC;

using SteamGiveawaysBot.Server.Api.Models;
using SteamGiveawaysBot.Server.DataAccess.DataObjects;
using SteamGiveawaysBot.Server.Security;
using SteamGiveawaysBot.Server.Service.Mapping;
using SteamGiveawaysBot.Server.Service.Models;

namespace SteamGiveawaysBot.Server.Service
{
    public sealed class SteamAccountService : ISteamAccountService
    {
        readonly IXmlRepository<UserEntity> userRepository;
        readonly IXmlRepository<SteamAccountEntity> steamAccountRepository;

        readonly IHmacEncoder<SteamAccountRequest> requestHmacEncoder;
        readonly IHmacEncoder<SteamAccountResponse> responseHmacEncoder;

        public SteamAccountService(
            IXmlRepository<UserEntity> userRepository,
            IXmlRepository<SteamAccountEntity> steamAccountRepository,
            IHmacEncoder<SteamAccountRequest> requestHmacEncoder,
            IHmacEncoder<SteamAccountResponse> responseHmacEncoder)
        {
            this.userRepository = userRepository;
            this.steamAccountRepository = steamAccountRepository;
            this.requestHmacEncoder = requestHmacEncoder;
            this.responseHmacEncoder = responseHmacEncoder;
        }

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
            bool isTokenValid = requestHmacEncoder.IsTokenValid(request.HmacToken, request, user.SharedSecretKey);

            if (!isTokenValid)
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
            SteamAccountResponse response = new SteamAccountResponse();
            response.Username = steamAccount.Username;
            response.Password = steamAccount.Password;
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

            SteamAccount randomAccount = steamAccounts.GetRandomElement();

            return randomAccount;
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
