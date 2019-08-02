using System.Net;
using System.Text.RegularExpressions;

using NuciLog;
using NuciLog.Core;

using SteamGiveawaysBot.Server.Client.Models;
using SteamGiveawaysBot.Server.Logging;

namespace SteamGiveawaysBot.Server.Client
{
    public sealed class StorefrontDataRetriever : IStorefrontDataRetriever
    {
        const string StorefrontApiUrl = "http://store.steampowered.com/api";
        const string StorefrontApiCountry = "RO";
        const string StorefrontApiFilters = "basic";

        readonly WebClient webClient;
        readonly ILogger logger;

        public StorefrontDataRetriever(ILogger logger)
        {
            webClient = new WebClient();

            this.logger = logger;
        }

        public SteamAppEntity GetAppData(string appId)
        {
            const string namePattern = "\"name\": *\"([^\"]*)\"";

            logger.Info(MyOperation.AppDataRetrieval, OperationStatus.Started, new LogInfo(MyLogInfoKey.AppId, appId));

            string endpoint = $"{StorefrontApiUrl}/appdetails?appids={appId}&cc={StorefrontApiCountry}&filters={StorefrontApiFilters}";
            string responseContent = webClient.DownloadString(endpoint);

            SteamAppEntity steamAppEntity = new SteamAppEntity();
            steamAppEntity.Id = appId;
            steamAppEntity.Name = Regex.Match(responseContent, namePattern).Groups[1].Value;

            logger.Debug(MyOperation.AppDataRetrieval, OperationStatus.Success, new LogInfo(MyLogInfoKey.AppId, appId));

            return steamAppEntity;
        }
    }
}
