using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using SteamGiveawaysBot.Server.DataAccess.DataObjects;
using SteamGiveawaysBot.Server.Service.Models;

namespace SteamGiveawaysBot.Server.Service.Mapping
{
    static class RewardMappingExtensions
    {
        internal static Reward ToDomainModel(this RewardDataObject dataObject) => new()
        {
            Id = dataObject.Id,
            GiveawaysProvider = dataObject.GiveawaysProvider,
            GiveawayId = dataObject.GiveawayId,
            SteamUsername = dataObject.SteamUsername,
            ActivationKey = dataObject.ActivationKey,
            SteamApp = new()
            {
                Id = dataObject.SteamAppId
            },
            CreationTime = DateTimeOffset.Parse(dataObject.CreationTimestamp, CultureInfo.InvariantCulture)
        };

        internal static RewardDataObject ToDataObject(this Reward domainModel) => new()
        {
            Id = domainModel.Id,
            GiveawaysProvider = domainModel.GiveawaysProvider,
            GiveawayId = domainModel.GiveawayId,
            SteamUsername = domainModel.SteamUsername,
            SteamAppId = domainModel.SteamApp.Id,
            ActivationKey = domainModel.ActivationKey,
            CreationTimestamp = domainModel.CreationTime.ToString(ValueFormats.DateTime)
        };

        internal static IEnumerable<Reward> ToDomainModels(this IEnumerable<RewardDataObject> dataObjects)
            => dataObjects.Select(dataObject => dataObject.ToDomainModel());

        internal static IEnumerable<RewardDataObject> ToDataObjects(this IEnumerable<Reward> domainModels)
            => domainModels.Select(domainModel => domainModel.ToDataObject());
    }
}
