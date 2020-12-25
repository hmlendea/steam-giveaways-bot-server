using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using SteamGiveawaysBot.Server.DataAccess.DataObjects;
using SteamGiveawaysBot.Server.Service.Models;

namespace SteamGiveawaysBot.Server.Service.Mapping
{
    static class SteamAccountMappings
    {
        const string DateTimeFormat = "yyyy.MM.ddTHH:mm:ss.ffffzzz";
        
        internal static SteamAccount ToServiceModel(this SteamAccountEntity dataObject)
        {
            SteamAccount serviceModel = new SteamAccount();
            serviceModel.Id = dataObject.Id;
            serviceModel.Username = dataObject.Username;
            serviceModel.Password = dataObject.Password;
            serviceModel.IsSteamGiftsSuspended = dataObject.IsSteamGiftsSuspended;

            serviceModel.CreationTime = DateTime.ParseExact(dataObject.CreationTimestamp, DateTimeFormat, CultureInfo.InvariantCulture);
            serviceModel.LastUpdateTime = DateTime.ParseExact(dataObject.LastUpdateTimestamp, DateTimeFormat, CultureInfo.InvariantCulture);

            return serviceModel;
        }

        internal static SteamAccountEntity ToDataObject(this SteamAccount serviceModel)
        {
            SteamAccountEntity dataObject = new SteamAccountEntity();
            dataObject.Id = serviceModel.Id;
            dataObject.Username = serviceModel.Username;
            dataObject.Password = serviceModel.Password;
            dataObject.IsSteamGiftsSuspended = serviceModel.IsSteamGiftsSuspended;

            dataObject.CreationTimestamp = serviceModel.CreationTime.ToString(DateTimeFormat);
            dataObject.LastUpdateTimestamp = serviceModel.LastUpdateTime.ToString(DateTimeFormat);

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
