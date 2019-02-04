using System.Threading.Tasks;

using SteamAccountDistributor.Api.Models;

namespace SteamAccountDistributor.Service
{
    public interface ISteamAccountService
    {
        SteamAccountResponse GetAccount(string username);
    }
}
