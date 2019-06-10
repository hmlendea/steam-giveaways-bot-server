using System.Collections.Generic;
using System.Linq;

using SteamGiveawaysBot.Server.DataAccess.DataObjects;
using SteamGiveawaysBot.Server.Service.Models;

namespace SteamGiveawaysBot.Server.Service.Mapping
{
    static class RewardMappings
    {
        internal static Reward ToServiceModel(this RewardEntity dataObject)
        {
            Reward serviceModel = new Reward();
            serviceModel.Id = dataObject.Id;
            serviceModel.GiveawaysProvider = dataObject.GiveawaysProvider;
            serviceModel.GiveawayId = dataObject.GiveawayId;
            serviceModel.SteamUsername = dataObject.SteamUsername;
            serviceModel.SteamAppId = dataObject.SteamAppId;
            serviceModel.ActivationKey = dataObject.ActivationKey;

            return serviceModel;
        }

        internal static RewardEntity ToDataObject(this Reward serviceModel)
        {
            RewardEntity dataObject = new RewardEntity();
            dataObject.Id = serviceModel.Id;
            dataObject.GiveawaysProvider = serviceModel.GiveawaysProvider;
            dataObject.GiveawayId = serviceModel.GiveawayId;
            dataObject.SteamUsername = serviceModel.SteamUsername;
            dataObject.SteamAppId = serviceModel.SteamAppId;
            dataObject.ActivationKey = serviceModel.ActivationKey;

            return dataObject;
        }

        internal static IEnumerable<Reward> ToServiceModels(this IEnumerable<RewardEntity> dataObjects)
        {
            IEnumerable<Reward> serviceModels = dataObjects.Select(dataObject => dataObject.ToServiceModel());

            return serviceModels;
        }

        internal static IEnumerable<RewardEntity> ToEntities(this IEnumerable<Reward> serviceModels)
        {
            IEnumerable<RewardEntity> dataObjects = serviceModels.Select(serviceModel => serviceModel.ToDataObject());

            return dataObjects;
        }
    }
}
