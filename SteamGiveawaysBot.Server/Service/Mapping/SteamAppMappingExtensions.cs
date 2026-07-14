using System.Collections.Generic;
using System.Linq;

using SteamGiveawaysBot.Server.Client.Models;
using SteamGiveawaysBot.Server.Service.Models;

namespace SteamGiveawaysBot.Server.Service.Mapping
{
    static class SteamAppMappingExtensions
    {
        internal static SteamApp ToDomainModel(this SteamAppDataObject dataObject) => new()
        {
            Id = dataObject.Id,
            Name = dataObject.Name
        };

        internal static SteamAppDataObject ToDataObject(this SteamApp domainModel) => new()
        {
            Id = domainModel.Id,
            Name = domainModel.Name
        };

        internal static IEnumerable<SteamApp> ToDomainModels(
            this IEnumerable<SteamAppDataObject> dataObjects)
            => dataObjects.Select(dataObject => dataObject.ToDomainModel());

        internal static IEnumerable<SteamAppDataObject> ToDataObjects(
            this IEnumerable<SteamApp> domainModels)
            => domainModels.Select(domainModel => domainModel.ToDataObject());
    }
}
