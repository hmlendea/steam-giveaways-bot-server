using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using SteamGiveawaysBot.Server.DataAccess.DataObjects;
using SteamGiveawaysBot.Server.Service.Models;

namespace SteamGiveawaysBot.Server.Service.Mapping
{
    static class UserMappingExtensions
    {
        internal static User ToDomainModel(this UserDataObject dataObject) => new()
        {
            Id = dataObject.Id,
            Username = dataObject.Username,
            SharedSecretKey = dataObject.SharedSecretKey,
            AssignedSteamAccount = dataObject.AssignedSteamAccount,
            IpAddress = dataObject.IpAddress,
            CreationTime = DateTimeOffset.Parse(dataObject.CreationTimestamp, CultureInfo.InvariantCulture),
            LastUpdateTime = DateTimeOffset.Parse(dataObject.LastUpdateTimestamp, CultureInfo.InvariantCulture)
        };

        internal static UserDataObject ToDataObject(this User domainModel) => new()
        {
            Id = domainModel.Id,
            Username = domainModel.Username,
            SharedSecretKey = domainModel.SharedSecretKey,
            AssignedSteamAccount = domainModel.AssignedSteamAccount,
            IpAddress = domainModel.IpAddress,
            CreationTimestamp = domainModel.CreationTime.ToString(ValueFormats.DateTime),
            LastUpdateTimestamp = domainModel.LastUpdateTime.ToString(ValueFormats.DateTime)
        };

        internal static IEnumerable<User> ToDomainModels(this IEnumerable<UserDataObject> dataObjects)
            => dataObjects.Select(dataObject => dataObject.ToDomainModel());

        internal static IEnumerable<UserDataObject> ToDataObjects(this IEnumerable<User> domainModels)
            => domainModels.Select(domainModel => domainModel.ToDataObject());
    }
}
