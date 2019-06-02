using System.Collections.Generic;
using System.Linq;

using SteamGiveawaysBot.Server.DataAccess.DataObjects;
using SteamGiveawaysBot.Server.Service.Models;

namespace SteamGiveawaysBot.Server.Service.Mapping
{
    static class UserMappings
    {
        internal static User ToServiceModel(this UserEntity dataObject)
        {
            User serviceModel = new User();
            serviceModel.Username = dataObject.Username;
            serviceModel.SharedSecretKey = dataObject.SharedSecretKey;
            serviceModel.AssignedSteamAccount = dataObject.AssignedSteamAccount;

            return serviceModel;
        }

        internal static UserEntity ToDataObject(this User serviceModel)
        {
            UserEntity dataObject = new UserEntity();
            dataObject.Username = serviceModel.Username;
            dataObject.SharedSecretKey = serviceModel.SharedSecretKey;
            dataObject.AssignedSteamAccount = serviceModel.AssignedSteamAccount;

            return dataObject;
        }

        internal static IEnumerable<User> ToServiceModels(this IEnumerable<UserEntity> dataObjects)
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
