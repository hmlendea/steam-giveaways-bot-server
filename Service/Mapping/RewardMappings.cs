using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using SteamGiveawaysBot.Server.DataAccess.DataObjects;
using SteamGiveawaysBot.Server.Service.Models;

namespace SteamGiveawaysBot.Server.Service.Mapping
{
    static class RewardMappings
    {
        const string DateTimeFormat = "yyyy.MM.ddTHH:mm:ss.ffffzzz";

        internal static Reward ToServiceModel(this RewardEntity dataObject) => new()
        {
            Id = dataObject.Id,
            GiveawaysProvider = dataObject.GiveawaysProvider,
            GiveawayId = dataObject.GiveawayId,
            SteamUsername = dataObject.SteamUsername,
            ActivationKey = dataObject.ActivationKey,
            SteamApp = new SteamApp
            {
                Id = dataObject.SteamAppId
            },
            CreationTime = DateTime.ParseExact(dataObject.CreationTimestamp, DateTimeFormat, CultureInfo.InvariantCulture)
        };

        internal static RewardEntity ToDataObject(this Reward serviceModel) => new()
        {
            Id = serviceModel.Id,
            GiveawaysProvider = serviceModel.GiveawaysProvider,
            GiveawayId = serviceModel.GiveawayId,
            SteamUsername = serviceModel.SteamUsername,
            SteamAppId = serviceModel.SteamApp.Id,
            ActivationKey = serviceModel.ActivationKey,
            CreationTimestamp = serviceModel.CreationTime.ToString(DateTimeFormat)
        };

        internal static IEnumerable<Reward> ToServiceModels(this IEnumerable<RewardEntity> dataObjects)
            => dataObjects.Select(dataObject => dataObject.ToServiceModel());

        internal static IEnumerable<RewardEntity> ToEntities(this IEnumerable<Reward> serviceModels)
            => serviceModels.Select(serviceModel => serviceModel.ToDataObject());
    }
}
