using System.Collections.Generic;
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

            IEnumerable<LogInfo> logInfos =
            [
                new(MyLogInfoKey.AppId, appId)
            ];

            logger.Info(
                MyOperation.AppDataRetrieval,
                OperationStatus.Started,
                logInfos);

            string endpoint = $"{StorefrontApiUrl}/appdetails?appids={appId}&cc={StorefrontApiCountry}&filters={StorefrontApiFilters}";
            string responseContent = httpClient.GetStringAsync(endpoint).GetAwaiter().GetResult();

            SteamAppEntity steamAppEntity = new()
            {
                Id = appId,
                Name = Regex.Match(responseContent, namePattern).Groups[1].Value
            };

            logger.Debug(
                MyOperation.AppDataRetrieval,
                OperationStatus.Success,
                logInfos);

            return steamAppEntity;
        }
    }
}
