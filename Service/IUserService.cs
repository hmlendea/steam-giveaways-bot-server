using SteamGiveawaysBot.Server.Requests;

namespace SteamGiveawaysBot.Server.Service
{
    public interface IUserService
    {
        void SetIpAddress(SetIpAddressRequest request);
    }
}
