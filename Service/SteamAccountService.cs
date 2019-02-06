using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;

using SteamAccountDistributor.Api.Models;
using SteamAccountDistributor.Core.Extensions;
using SteamAccountDistributor.DataAccess.Repositories;
using SteamAccountDistributor.Service.Mapping;
using SteamAccountDistributor.Service.Models;

namespace SteamAccountDistributor.Service
{
    public sealed class SteamAccountService : ISteamAccountService
    {
        readonly IUserRepository userRepository;

        readonly ISteamAccountRepository steamAccountRepository;

        public SteamAccountService(
            IUserRepository userRepository,
            ISteamAccountRepository steamAccountRepository)
        {
            this.userRepository = userRepository;
            this.steamAccountRepository = steamAccountRepository;
        }

        public SteamAccountResponse GetAccount(SteamAccountRequest request)
        {
            User user = userRepository.Get(request.Username).ToServiceModel();
            ValidateRequest(request, user);

            SteamAccount assignedAccount = GetAssignedAccount(user, request.GiveawaysProvider);
            SteamAccountResponse response = new SteamAccountResponse
            {
                Username = assignedAccount.Username,
                Password = assignedAccount.Password
            };

            return response;
        }

        void ValidateRequest(SteamAccountRequest request, User user)
        {
            if (user.Password != request.Password)
            {
                throw new AuthenticationException($"Incorrect password");
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

        SteamAccount FindAccountToAssign(string gaProvider)
        {
            IEnumerable<User> users = userRepository.GetAll().ToServiceModels();
            IEnumerable<SteamAccount> steamAccounts = steamAccountRepository.GetAll().ToServiceModels();

            steamAccounts = steamAccounts.Where(x => users.All(y => y.AssignedSteamAccount != x.Username));

            if (gaProvider.Equals("STEAMGIFITS"))
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

            if (gaProvider.Equals("STEAMGIFTS") && account.IsSteamGiftsSuspended)
            {
                return true;
            }

            return false;
        }
    }
}
