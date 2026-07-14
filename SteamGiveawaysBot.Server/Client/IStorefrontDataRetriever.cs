using SteamGiveawaysBot.Server.Client.Models;

namespace SteamGiveawaysBot.Server.Client
{
    public interface IStorefrontDataRetriever
    {
        SteamAppDataObject GetAppData(string appId);
    }
}
