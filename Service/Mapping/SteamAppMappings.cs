using System.Collections.Generic;
using System.Linq;

using SteamGiveawaysBot.Server.Client.Models;
using SteamGiveawaysBot.Server.Service.Models;

namespace SteamGiveawaysBot.Server.Service.Mapping
{
    static class SteamAppMappings
    {
        internal static SteamApp ToServiceModel(this SteamAppEntity dataObject) => new()
        {
            Id = dataObject.Id,
            Name = dataObject.Name
        };

        internal static SteamAppEntity ToDataObject(this SteamApp serviceModel) => new()
        {
            Id = serviceModel.Id,
            Name = serviceModel.Name
        };

        internal static IEnumerable<SteamApp> ToServiceModels(this IEnumerable<SteamAppEntity> dataObjects)
            => dataObjects.Select(dataObject => dataObject.ToServiceModel());

        internal static IEnumerable<SteamAppEntity> ToEntities(this IEnumerable<SteamApp> serviceModels)
            => serviceModels.Select(serviceModel => serviceModel.ToDataObject());
    }
}
