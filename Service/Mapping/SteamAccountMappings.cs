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

        internal static SteamAccount ToServiceModel(this SteamAccountEntity dataObject) => new()
        {
            Id = dataObject.Id,
            Username = dataObject.Username,
            Password = dataObject.Password,
            IsSteamGiftsSuspended = dataObject.IsSteamGiftsSuspended,
            CreationTime = DateTime.ParseExact(dataObject.CreationTimestamp, DateTimeFormat, CultureInfo.InvariantCulture),
            LastUpdateTime = DateTime.ParseExact(dataObject.LastUpdateTimestamp, DateTimeFormat, CultureInfo.InvariantCulture)
        };

        internal static SteamAccountEntity ToDataObject(this SteamAccount serviceModel) => new()
        {
            Id = serviceModel.Id,
            Username = serviceModel.Username,
            Password = serviceModel.Password,
            IsSteamGiftsSuspended = serviceModel.IsSteamGiftsSuspended,
            CreationTimestamp = serviceModel.CreationTime.ToString(DateTimeFormat),
            LastUpdateTimestamp = serviceModel.LastUpdateTime.ToString(DateTimeFormat)
        };

        internal static IEnumerable<SteamAccount> ToServiceModels(this IEnumerable<SteamAccountEntity> dataObjects)
            => dataObjects.Select(dataObject => dataObject.ToServiceModel());

        internal static IEnumerable<SteamAccountEntity> ToEntities(this IEnumerable<SteamAccount> serviceModels)
            => serviceModels.Select(serviceModel => serviceModel.ToDataObject());
    }
}
