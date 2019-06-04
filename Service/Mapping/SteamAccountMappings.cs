using System.Collections.Generic;
using System.Linq;

using SteamGiveawaysBot.Server.DataAccess.DataObjects;
using SteamGiveawaysBot.Server.Service.Models;

namespace SteamGiveawaysBot.Server.Service.Mapping
{
    static class SteamAccountMappings
    {
        internal static SteamAccount ToServiceModel(this SteamAccountEntity dataObject)
        {
            SteamAccount serviceModel = new SteamAccount();
            serviceModel.Id = dataObject.Id;
            serviceModel.Username = dataObject.Username;
            serviceModel.Password = dataObject.Password;
            serviceModel.IsSteamGiftsSuspended = dataObject.IsSteamGiftsSuspended;

            return serviceModel;
        }

        internal static SteamAccountEntity ToDataObject(this SteamAccount serviceModel)
        {
            SteamAccountEntity dataObject = new SteamAccountEntity();
            dataObject.Id = serviceModel.Id;
            dataObject.Username = serviceModel.Username;
            dataObject.Password = serviceModel.Password;
            dataObject.IsSteamGiftsSuspended = serviceModel.IsSteamGiftsSuspended;

            return dataObject;
        }

        internal static IEnumerable<SteamAccount> ToServiceModels(this IEnumerable<SteamAccountEntity> dataObjects)
        {
            IEnumerable<SteamAccount> serviceModels = dataObjects.Select(dataObject => dataObject.ToServiceModel());

            return serviceModels;
        }

        internal static IEnumerable<SteamAccountEntity> ToEntities(this IEnumerable<SteamAccount> serviceModels)
        {
            IEnumerable<SteamAccountEntity> dataObjects = serviceModels.Select(serviceModel => serviceModel.ToDataObject());

            return dataObjects;
        }
    }
}
