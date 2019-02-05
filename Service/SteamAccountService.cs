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

            SteamAccount assignedAccount = GetAssignedAccount(user);
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

        SteamAccount GetAssignedAccount(User user)
        {
            bool needsReuser = DoesItNeedReuser(user);
            SteamAccount assignedAccount;

            if (needsReuser)
            {
                assignedAccount = FindAccountToAssign();

                user.AssignedSteamAccount = assignedAccount.Username;
                userRepository.Update(user.ToDataObject());
            }
            else
            {
                assignedAccount = steamAccountRepository.Get(user.AssignedSteamAccount).ToServiceModel();
            }

            return assignedAccount;
        }

        SteamAccount FindAccountToAssign()
        {
            IEnumerable<User> users = userRepository.GetAll().ToServiceModels();
            IEnumerable<SteamAccount> steamAccounts = steamAccountRepository.GetAll().ToServiceModels();

            SteamAccount randomAccount = steamAccounts
                .Where(x => users.All(y => y.AssignedSteamAccount != x.Username))
                .GetRandomElement();

            return randomAccount;
        }

        bool DoesItNeedReuser(User user)
        {
            if (string.IsNullOrWhiteSpace(user.AssignedSteamAccount))
            {
                return true;
            }

            return false;
        }
    }
}
