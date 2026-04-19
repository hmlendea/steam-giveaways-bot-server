using System.Net.Http;
using System.Text.RegularExpressions;

using NuciLog.Core;

using SteamGiveawaysBot.Server.Client.Models;
using SteamGiveawaysBot.Server.Logging;

namespace SteamGiveawaysBot.Server.Client
{
    public sealed class StorefrontDataRetriever(ILogger logger) : IStorefrontDataRetriever
    {
        const string StorefrontApiUrl = "http://store.steampowered.com/api";
        const string StorefrontApiCountry = "RO";
        const string StorefrontApiFilters = "basic";

        readonly HttpClient httpClient = new();
        readonly ILogger logger = logger;

        public SteamAppEntity GetAppData(string appId)
        {
            const string namePattern = "\"name\": *\"([^\"]*)\"";

            logger.Info(MyOperation.AppDataRetrieval, OperationStatus.Started, new LogInfo(MyLogInfoKey.AppId, appId));

            string endpoint = $"{StorefrontApiUrl}/appdetails?appids={appId}&cc={StorefrontApiCountry}&filters={StorefrontApiFilters}";
            string responseContent = httpClient.GetStringAsync(endpoint).GetAwaiter().GetResult();

            SteamAppEntity steamAppEntity = new()
            {
                Id = appId,
                Name = Regex.Match(responseContent, namePattern).Groups[1].Value
            };

            logger.Debug(MyOperation.AppDataRetrieval, OperationStatus.Success, new LogInfo(MyLogInfoKey.AppId, appId));

            return steamAppEntity;
        }
    }
}
