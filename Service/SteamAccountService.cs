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
        readonly IAssignmentRepository assignmentRepository;

        readonly ISteamAccountRepository steamAccountRepository;

        public SteamAccountService(
            IAssignmentRepository assignmentRepository,
            ISteamAccountRepository steamAccountRepository)
        {
            this.assignmentRepository = assignmentRepository;
            this.steamAccountRepository = steamAccountRepository;
        }

        public SteamAccountResponse GetAccount(string hostname, string password)
        {
            Assignment assignment = assignmentRepository.Get(hostname).ToServiceModel();

            if (assignment.Password != password)
            {
                throw new AuthenticationException($"Incorrect password");
            }

            if (string.IsNullOrWhiteSpace(assignment.AssignedSteamAccount))
            {
                // TODO: Make sure that the new account is not already assigned to some other user
                assignment.AssignedSteamAccount = steamAccountRepository.GetAll().GetRandomElement().Username;

                assignmentRepository.Update(assignment.ToDataObject());
            }

            SteamAccount steamAccount = steamAccountRepository.Get(assignment.AssignedSteamAccount).ToServiceModel();

            SteamAccountResponse response = new SteamAccountResponse
            {
                Username = steamAccount.Username,
                Password = steamAccount.Password
            };

            return response;
        }
    }
}
