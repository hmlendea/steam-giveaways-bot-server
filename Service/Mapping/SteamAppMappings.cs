using System.Collections.Generic;
using System.Linq;

using SteamGiveawaysBot.Server.Client.Models;
using SteamGiveawaysBot.Server.Service.Models;

namespace SteamGiveawaysBot.Server.Service.Mapping
{
    static class SteamAppMappings
    {
        internal static SteamApp ToServiceModel(this SteamAppEntity dataObject)
        {
            SteamApp serviceModel = new SteamApp();
            serviceModel.Id = dataObject.Id;
            serviceModel.Name = dataObject.Name;

            return serviceModel;
        }

        internal static SteamAppEntity ToDataObject(this SteamApp serviceModel)
        {
            SteamAppEntity dataObject = new SteamAppEntity();
            dataObject.Id = serviceModel.Id;
            dataObject.Name = serviceModel.Name;

            return dataObject;
        }

        internal static IEnumerable<SteamApp> ToServiceModels(this IEnumerable<SteamAppEntity> dataObjects)
        {
            IEnumerable<SteamApp> serviceModels = dataObjects.Select(dataObject => dataObject.ToServiceModel());

            return serviceModels;
        }

        internal static IEnumerable<SteamAppEntity> ToEntities(this IEnumerable<SteamApp> serviceModels)
        {
            IEnumerable<SteamAppEntity> dataObjects = serviceModels.Select(serviceModel => serviceModel.ToDataObject());

            return dataObjects;
        }
    }
}
