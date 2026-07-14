using SteamGiveawaysBot.Server.Requests;
using SteamGiveawaysBot.Server.Responses;

namespace SteamGiveawaysBot.Server.Service
{
    public interface ISteamAccountService
    {
        GetSteamAccountResponse GetAccount(GetSteamAccountRequest request);
    }
}
