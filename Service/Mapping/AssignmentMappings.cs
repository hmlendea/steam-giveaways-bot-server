using System.Collections.Generic;
using System.Linq;

using SteamAccountDistributor.DataAccess.DataObjects;
using SteamAccountDistributor.Service.Models;

namespace SteamAccountDistributor.Service.Mapping
{
    static class AssignmentMappings
    {
        internal static Assignment ToServiceModel(this AssignmentEntity dataObject)
        {
            Assignment serviceModel = new Assignment();
            serviceModel.Username = dataObject.Hostname;
            serviceModel.Password = dataObject.Password;
            serviceModel.AssignedSteamAccount = dataObject.AssignedSteamAccount;

            return serviceModel;
        }

        internal static AssignmentEntity ToDataObject(this Assignment serviceModel)
        {
            AssignmentEntity dataObject = new AssignmentEntity();
            dataObject.Hostname = serviceModel.Username;
            dataObject.Password = serviceModel.Password;
            dataObject.AssignedSteamAccount = serviceModel.AssignedSteamAccount;

            return dataObject;
        }

        internal static IEnumerable<Assignment> ToServiceModels(this IEnumerable<AssignmentEntity> dataObjects)
        {
            IEnumerable<Assignment> serviceModels = dataObjects.Select(dataObject => dataObject.ToServiceModel());

            return serviceModels;
        }

        internal static IEnumerable<AssignmentEntity> ToEntities(this IEnumerable<Assignment> serviceModels)
        {
            IEnumerable<AssignmentEntity> dataObjects = serviceModels.Select(serviceModel => serviceModel.ToDataObject());

            return dataObjects;
        }
    }
}
