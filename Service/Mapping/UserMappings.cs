using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using SteamGiveawaysBot.Server.DataAccess.DataObjects;
using SteamGiveawaysBot.Server.Service.Models;

namespace SteamGiveawaysBot.Server.Service.Mapping
{
    static class UserMappings
    {
        const string DateTimeFormat = "yyyy.MM.ddTHH:mm:ss.ffffzzz";

        internal static User ToServiceModel(this UserEntity dataObject) => new()
        {
            Id = dataObject.Id,
            Username = dataObject.Username,
            SharedSecretKey = dataObject.SharedSecretKey,
            AssignedSteamAccount = dataObject.AssignedSteamAccount,
            IpAddress = dataObject.IpAddress,
            CreationTime = DateTime.ParseExact(dataObject.CreationTimestamp, DateTimeFormat, CultureInfo.InvariantCulture),
            LastUpdateTime = DateTime.ParseExact(dataObject.LastUpdateTimestamp, DateTimeFormat, CultureInfo.InvariantCulture)
        };

        internal static UserEntity ToDataObject(this User serviceModel) => new()
        {
            Id = serviceModel.Id,
            Username = serviceModel.Username,
            SharedSecretKey = serviceModel.SharedSecretKey,
            AssignedSteamAccount = serviceModel.AssignedSteamAccount,
            IpAddress = serviceModel.IpAddress,
            CreationTimestamp = serviceModel.CreationTime.ToString(DateTimeFormat),
            LastUpdateTimestamp = serviceModel.LastUpdateTime.ToString(DateTimeFormat)
        };

        internal static IEnumerable<User> ToServiceModels(this IEnumerable<UserEntity> dataObjects)
            => dataObjects.Select(dataObject => dataObject.ToServiceModel());

        internal static IEnumerable<UserEntity> ToEntities(this IEnumerable<User> serviceModels)
            => serviceModels.Select(serviceModel => serviceModel.ToDataObject());
    }
}
