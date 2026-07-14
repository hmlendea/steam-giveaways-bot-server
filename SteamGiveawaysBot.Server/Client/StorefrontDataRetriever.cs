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
        private readonly HttpClient httpClient = new();

        public SteamAppDataObject GetAppData(string appId)
        {
            IEnumerable<LogInfo> logInfos =
            [
                new(MyLogInfoKey.AppId, appId)
            ];

            logger.Info(
                MyOperation.AppDataRetrieval,
                OperationStatus.Started,
                logInfos);

            string endpoint = $"{StorefrontApiUrl}/appdetails"
                + $"?appids={appId}"
                + $"&cc={StorefrontApiCountry}"
                + $"&filters={StorefrontApiFilters}";

            string responseContent = httpClient.GetStringAsync(endpoint).GetAwaiter().GetResult();

            SteamAppDataObject steamAppData = new()
            {
                Id = appId,
                Name = Regex.Match(responseContent, AppNamePattern).Groups[1].Value
            };

            logger.Debug(
                MyOperation.AppDataRetrieval,
                OperationStatus.Success,
                logInfos);

            return steamAppData;
        }

        private static string StorefrontApiUrl => "http://store.steampowered.com/api";

        private static string StorefrontApiCountry => "RO";

        private static string StorefrontApiFilters => "basic";

        private static string AppNamePattern => "\"name\": *\"([^\"]*)\"";
    }
}
