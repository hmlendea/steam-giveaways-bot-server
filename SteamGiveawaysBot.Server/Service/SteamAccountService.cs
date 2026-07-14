using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;

using NuciDAL.Repositories;

using NuciExtensions;

using NuciSecurity.HMAC;

using SteamGiveawaysBot.Server.DataAccess.DataObjects;
using SteamGiveawaysBot.Server.Requests;
using SteamGiveawaysBot.Server.Responses;
using SteamGiveawaysBot.Server.Service.Mapping;
using SteamGiveawaysBot.Server.Service.Models;

namespace SteamGiveawaysBot.Server.Service
{
    public sealed class SteamAccountService(
        IFileRepository<UserDataObject> userRepository,
        IFileRepository<SteamAccountDataObject> steamAccountRepository) : ISteamAccountService
    {
        private static string SteamGiftsProviderName => "SteamGifts";

        public GetSteamAccountResponse GetAccount(GetSteamAccountRequest request)
        {
            User user = userRepository.Get(request.Username).ToDomainModel();

            ValidateRequest(request, user);

            SteamAccount assignedAccount = GetAssignedAccount(user, request.GiveawaysProvider);
            GetSteamAccountResponse response = CreateResponse(user, assignedAccount);

            return response;
        }

        private static void ValidateRequest(GetSteamAccountRequest request, User user)
        {
            if (!HmacValidator.IsTokenValid(request.HmacToken, request, user.SharedSecretKey))
            {
                throw new AuthenticationException("The provided HMAC token is not valid");
            }
        }

        private SteamAccount GetAssignedAccount(User user, string giveawaysProvider)
        {
            if (DoesUserNeedAccountReassignment(user, giveawaysProvider))
            {
                SteamAccount newAccount = FindAccountToAssign(giveawaysProvider);

                user.AssignedSteamAccount = newAccount.Username;
                userRepository.Update(user.ToDataObject());
                userRepository.SaveChanges();

                return newAccount;
            }

            return steamAccountRepository.Get(user.AssignedSteamAccount).ToDomainModel();
        }

        private static GetSteamAccountResponse CreateResponse(User user, SteamAccount steamAccount)
        {
            GetSteamAccountResponse response = new()
            {
                Username = steamAccount.Username,
                Password = steamAccount.Password
            };

            response.HmacToken = HmacEncoder.GenerateToken(response, user.SharedSecretKey);

            return response;
        }

        private SteamAccount FindAccountToAssign(string giveawaysProvider)
        {
            IEnumerable<User> users = userRepository.GetAll().ToDomainModels();
            IEnumerable<SteamAccount> steamAccounts = steamAccountRepository.GetAll().ToDomainModels();

            steamAccounts = steamAccounts.Where(
                account => users.All(
                    user => !string.Equals(user.AssignedSteamAccount, account.Username)));

            bool isSteamGiftsProvider = string.Equals(
                giveawaysProvider,
                SteamGiftsProviderName,
                StringComparison.InvariantCultureIgnoreCase);

            if (isSteamGiftsProvider)
            {
                steamAccounts = steamAccounts.Where(account => !account.IsSteamGiftsSuspended);
            }

            return steamAccounts.GetRandomElement();
        }

        private bool DoesUserNeedAccountReassignment(User user, string giveawaysProvider)
        {
            if (string.IsNullOrWhiteSpace(user.AssignedSteamAccount))
            {
                return true;
            }

            SteamAccount account = steamAccountRepository.Get(user.AssignedSteamAccount).ToDomainModel();

            bool isSteamGiftsProvider = string.Equals(
                giveawaysProvider,
                SteamGiftsProviderName,
                StringComparison.InvariantCultureIgnoreCase);

            if (isSteamGiftsProvider && account.IsSteamGiftsSuspended)
            {
                return true;
            }

            return false;
        }
    }
}
