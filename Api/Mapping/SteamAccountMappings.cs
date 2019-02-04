using System.Collections.Generic;
using System.Linq;

using SteamAccountDistributor.Api.Models;
using SteamAccountDistributor.DataAccess.DataObjects;

namespace SteamAccountDistributor.Api.Mapping
{
    static class SteamAccountMappingExtensions
    {
        internal static SteamAccount ToApiModel(this SteamAccountEntity dataObject)
        {
            SteamAccount apiModel = new SteamAccount();
            apiModel.Username = dataObject.Username;
            apiModel.Password = dataObject.Password;

            return apiModel;
        }

        internal static SteamAccountEntity ToDataObject(this SteamAccount apiModel)
        {
            SteamAccountEntity dataObject = new SteamAccountEntity();
            dataObject.Username = apiModel.Username;
            dataObject.Password = apiModel.Password;

            return dataObject;
        }

        internal static IEnumerable<SteamAccount> ToApiModels(this IEnumerable<SteamAccountEntity> dataObjects)
        {
            IEnumerable<SteamAccount> apiModels = dataObjects.Select(dataObject => dataObject.ToApiModel());

            return apiModels;
        }

        internal static IEnumerable<SteamAccountEntity> ToEntities(this IEnumerable<SteamAccount> apiModels)
        {
            IEnumerable<SteamAccountEntity> dataObjects = apiModels.Select(apiModel => apiModel.ToDataObject());

            return dataObjects;
        }
    }
}
