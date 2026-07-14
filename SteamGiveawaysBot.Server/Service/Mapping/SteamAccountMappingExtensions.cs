using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using SteamGiveawaysBot.Server.DataAccess.DataObjects;
using SteamGiveawaysBot.Server.Service.Models;

namespace SteamGiveawaysBot.Server.Service.Mapping
{
    static class SteamAccountMappingExtensions
    {
        internal static SteamAccount ToDomainModel(this SteamAccountDataObject dataObject) => new()
        {
            Id = dataObject.Id,
            Username = dataObject.Username,
            Password = dataObject.Password,
            IsSteamGiftsSuspended = dataObject.IsSteamGiftsSuspended,
            CreationTime = DateTimeOffset.Parse(dataObject.CreationTimestamp, CultureInfo.InvariantCulture),
            LastUpdateTime = DateTimeOffset.Parse(dataObject.LastUpdateTimestamp, CultureInfo.InvariantCulture)
        };

        internal static SteamAccountDataObject ToDataObject(this SteamAccount domainModel) => new()
        {
            Id = domainModel.Id,
            Username = domainModel.Username,
            Password = domainModel.Password,
            IsSteamGiftsSuspended = domainModel.IsSteamGiftsSuspended,
            CreationTimestamp = domainModel.CreationTime.ToString(ValueFormats.DateTime),
            LastUpdateTimestamp = domainModel.LastUpdateTime.ToString(ValueFormats.DateTime)
        };

        internal static IEnumerable<SteamAccount> ToDomainModels(
            this IEnumerable<SteamAccountDataObject> dataObjects)
            => dataObjects.Select(dataObject => dataObject.ToDomainModel());

        internal static IEnumerable<SteamAccountDataObject> ToDataObjects(
            this IEnumerable<SteamAccount> domainModels)
            => domainModels.Select(domainModel => domainModel.ToDataObject());
    }
}
