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
        readonly IAssignmentRepository assignmentRepository;

        readonly ISteamAccountRepository steamAccountRepository;

        public SteamAccountService(
            IAssignmentRepository assignmentRepository,
            ISteamAccountRepository steamAccountRepository)
        {
            this.assignmentRepository = assignmentRepository;
            this.steamAccountRepository = steamAccountRepository;
        }

        public SteamAccountResponse GetAccount(SteamAccountRequest request)
        {
            Assignment assignment = assignmentRepository.Get(request.Hostname).ToServiceModel();
            ValidateRequest(request, assignment);

            SteamAccount assignedAccount = GetAssignedAccount(assignment, request.AccountStatus);
            SteamAccountResponse response = new SteamAccountResponse
            {
                Username = assignedAccount.Username,
                Password = assignedAccount.Password
            };

            return response;
        }

        void ValidateRequest(SteamAccountRequest request, Assignment assignment)
        {
            if (assignment.Password != request.Password)
            {
                throw new AuthenticationException($"Incorrect password");
            }
        }

        SteamAccount GetAssignedAccount(Assignment assignment, AccountStatus accountStatus)
        {
            bool needsReassignment = DoesItNeedReassignment(assignment, accountStatus);
            SteamAccount assignedAccount;

            if (needsReassignment)
            {
                assignedAccount = FindAccountToAssign();

                assignment.AssignedSteamAccount = assignedAccount.Username;
                assignmentRepository.Update(assignment.ToDataObject());
            }
            else
            {
                assignedAccount = steamAccountRepository.Get(assignment.AssignedSteamAccount).ToServiceModel();
            }

            return assignedAccount;
        }

        SteamAccount FindAccountToAssign()
        {
            IEnumerable<Assignment> assignments = assignmentRepository.GetAll().ToServiceModels();
            IEnumerable<SteamAccount> steamAccounts = steamAccountRepository.GetAll().ToServiceModels();

            SteamAccount randomAccount = steamAccounts
                .Where(x => assignments.All(y => y.AssignedSteamAccount != x.Username))
                .GetRandomElement();

            return randomAccount;
        }

        bool DoesItNeedReassignment(Assignment assignment, AccountStatus accountStatus)
        {
            if (string.IsNullOrWhiteSpace(assignment.AssignedSteamAccount) ||
                accountStatus == AccountStatus.Suspended)
            {
                return true;
            }

            return false;
        }
    }
}
