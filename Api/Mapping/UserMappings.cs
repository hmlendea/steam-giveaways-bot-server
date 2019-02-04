using System.Collections.Generic;
using System.Linq;

using SteamAccountDistributor.Api.Models;
using SteamAccountDistributor.DataAccess.DataObjects;

namespace SteamAccountDistributor.Api.Mapping
{
    static class UserMappingExtensions
    {
        internal static User ToApiModel(this UserEntity dataObject)
        {
            User apiModel = new User();
            apiModel.Username = dataObject.Username;
            apiModel.Password = dataObject.Password;
            apiModel.AssignedSteamAccount = dataObject.AssignedSteamAccount;

            return apiModel;
        }

        internal static UserEntity ToDataObject(this User apiModel)
        {
            UserEntity dataObject = new UserEntity();
            dataObject.Username = apiModel.Username;
            dataObject.Password = apiModel.Password;
            dataObject.AssignedSteamAccount = apiModel.AssignedSteamAccount;

            return dataObject;
        }

        internal static IEnumerable<User> ToApiModels(this IEnumerable<UserEntity> dataObjects)
        {
            IEnumerable<User> apiModels = dataObjects.Select(dataObject => dataObject.ToApiModel());

            return apiModels;
        }

        internal static IEnumerable<UserEntity> ToEntities(this IEnumerable<User> apiModels)
        {
            IEnumerable<UserEntity> dataObjects = apiModels.Select(apiModel => apiModel.ToDataObject());

            return dataObjects;
        }
    }
}
