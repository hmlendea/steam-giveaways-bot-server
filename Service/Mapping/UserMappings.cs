using System.Collections.Generic;
using System.Linq;

using SteamAccountDistributor.DataAccess.DataObjects;
using SteamAccountDistributor.Service.Models;

namespace SteamAccountDistributor.Service.Mapping
{
    static class UserMappingExtensions
    {
        internal static User ToServiceModel(this UserEntity dataObject)
        {
            User serviceModel = new User();
            serviceModel.Username = dataObject.Username;
            serviceModel.Password = dataObject.Password;
            serviceModel.AssignedSteamAccount = dataObject.AssignedSteamAccount;

            return serviceModel;
        }

        internal static UserEntity ToDataObject(this User serviceModel)
        {
            UserEntity dataObject = new UserEntity();
            dataObject.Username = serviceModel.Username;
            dataObject.Password = serviceModel.Password;
            dataObject.AssignedSteamAccount = serviceModel.AssignedSteamAccount;

            return dataObject;
        }

        internal static IEnumerable<User> ToApiModels(this IEnumerable<UserEntity> dataObjects)
        {
            IEnumerable<User> serviceModels = dataObjects.Select(dataObject => dataObject.ToServiceModel());

            return serviceModels;
        }

        internal static IEnumerable<UserEntity> ToEntities(this IEnumerable<User> serviceModels)
        {
            IEnumerable<UserEntity> dataObjects = serviceModels.Select(serviceModel => serviceModel.ToDataObject());

            return dataObjects;
        }
    }
}
