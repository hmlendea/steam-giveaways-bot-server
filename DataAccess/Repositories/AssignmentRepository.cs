using System.Collections.Generic;
using System.IO;
using System.Linq;

using SteamAccountDistributor.Core.Configuration;
using SteamAccountDistributor.DataAccess.Exceptions;
using SteamAccountDistributor.DataAccess.DataObjects;

namespace SteamAccountDistributor.DataAccess.Repositories
{
    public sealed class AssignmentRepository : IAssignmentRepository
    {
        const char CsvSeparator = ',';

        readonly SteamAccountDistributorConfiguration configuration;

        public AssignmentRepository(SteamAccountDistributorConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public IEnumerable<AssignmentEntity> GetAll()
        {
            IEnumerable<string> lines = File.ReadAllLines(configuration.AssignmentStorePath);
            IList<AssignmentEntity> assignments = new List<AssignmentEntity>();

            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }
    
                AssignmentEntity assignment = ReadAssignmentEntity(line);
                assignments.Add(assignment);
            }

            return assignments;
        }    

        public AssignmentEntity Get(string hostname)
        {
            IEnumerable<AssignmentEntity> assignments = GetAll();
            AssignmentEntity assignment = assignments.FirstOrDefault(x => x.Hostname == hostname);

            if (assignment == null)
            {
                throw new EntityNotFoundException(hostname, nameof(AssignmentEntity));
            }

            return assignment;
        }

        public void Update(AssignmentEntity assignment)
        {
            IEnumerable<AssignmentEntity> assignments = GetAll();
            AssignmentEntity oldAssignment = assignments.FirstOrDefault(x => x.Hostname == assignment.Hostname);

            if (oldAssignment == null)
            {
                throw new EntityNotFoundException(assignment.Hostname, nameof(AssignmentEntity));
            }

            oldAssignment.Password = assignment.Password;
            oldAssignment.AssignedSteamAccount = assignment.AssignedSteamAccount;

            IEnumerable<string> csvLines = assignments.Select(x => $"{x.Hostname},{x.Password},{x.AssignedSteamAccount}");
            File.WriteAllLines(configuration.AssignmentStorePath, csvLines);
        }

        public static AssignmentEntity ReadAssignmentEntity(string csvLine)
        {
            string[] fields = csvLine.Split(CsvSeparator);

            AssignmentEntity assignment = new AssignmentEntity();
            assignment.Hostname = fields[0];
            assignment.Password = fields[1];
            assignment.AssignedSteamAccount = fields[2];

            return assignment;
        }
    }
}
