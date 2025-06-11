using SteamGiveawaysBot.Server.Api.Models;

namespace SteamGiveawaysBot.Server.Service
{
    public interface IUserService
    {
        void SetIpAddress(SetIpAddressRequest request);
    }
}
