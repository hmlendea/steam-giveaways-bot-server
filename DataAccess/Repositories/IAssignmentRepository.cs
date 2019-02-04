using System.Collections.Generic;

using SteamAccountDistributor.DataAccess.DataObjects;

namespace SteamAccountDistributor.DataAccess.Repositories
{
    public interface IAssignmentRepository
    {
        IEnumerable<AssignmentEntity> GetAll();

        AssignmentEntity Get(string hostname);

        void Update(AssignmentEntity assignment);
    }
}
