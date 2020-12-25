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
        
        internal static User ToServiceModel(this UserEntity dataObject)
        {
            User serviceModel = new User();
            serviceModel.Id = dataObject.Id;
            serviceModel.Username = dataObject.Username;
            serviceModel.SharedSecretKey = dataObject.SharedSecretKey;
            serviceModel.AssignedSteamAccount = dataObject.AssignedSteamAccount;

            serviceModel.CreationTime = DateTime.ParseExact(dataObject.CreationTimestamp, DateTimeFormat, CultureInfo.InvariantCulture);
            serviceModel.LastUpdateTime = DateTime.ParseExact(dataObject.LastUpdateTimestamp, DateTimeFormat, CultureInfo.InvariantCulture);

            return serviceModel;
        }

        internal static UserEntity ToDataObject(this User serviceModel)
        {
            UserEntity dataObject = new UserEntity();
            dataObject.Id = serviceModel.Id;
            dataObject.Username = serviceModel.Username;
            dataObject.SharedSecretKey = serviceModel.SharedSecretKey;
            dataObject.AssignedSteamAccount = serviceModel.AssignedSteamAccount;

            dataObject.CreationTimestamp = serviceModel.CreationTime.ToString(DateTimeFormat);
            dataObject.LastUpdateTimestamp = serviceModel.LastUpdateTime.ToString(DateTimeFormat);

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
