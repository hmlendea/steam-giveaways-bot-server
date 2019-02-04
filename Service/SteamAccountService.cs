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

        public SteamAccountResponse GetAccount(string username)
        {
            User user = userRepository.Get(username).ToServiceModel();

            if (string.IsNullOrWhiteSpace(user.AssignedSteamAccount))
            {
                // TODO: Make sure that the new account is not already assigned to some other user
                user.AssignedSteamAccount = steamAccountRepository.GetAll().GetRandomElement().Username;

                userRepository.Update(user.ToDataObject());
            }

            SteamAccount steamAccount = steamAccountRepository.Get(user.AssignedSteamAccount).ToServiceModel();

            SteamAccountResponse response = new SteamAccountResponse
            {
                Username = steamAccount.Username,
                Password = steamAccount.Password
            };

            return response;
        }
    }
}
