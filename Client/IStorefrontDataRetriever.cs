using SteamGiveawaysBot.Server.Client.Models;

namespace SteamGiveawaysBot.Server.Client
{
    public interface IStorefrontDataRetriever
    {
        SteamAppEntity GetAppData(string appId);
    }
}
