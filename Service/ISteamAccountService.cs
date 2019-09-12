using SteamGiveawaysBot.Server.Api.Models;

namespace SteamGiveawaysBot.Server.Service
{
    public interface ISteamAccountService
    {
        SteamAccountResponse GetAccount(SteamAccountRequest request);
    }
}
