using SteamGiveawaysBot.Server.Api.Models;

namespace SteamGiveawaysBot.Server.Service
{
    public interface ISteamAccountService
    {
        GetSteamAccountResponse GetAccount(GetSteamAccountRequest request);
    }
}
