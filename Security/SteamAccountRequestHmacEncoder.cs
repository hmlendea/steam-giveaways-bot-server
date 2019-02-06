using SteamAccountDistributor.Api.Models;

namespace SteamAccountDistributor.Security
{
    public sealed class SteamAccountRequestHmacEncoder : HmacEncoder<SteamAccountRequest>
    {
        public override string GenerateToken(SteamAccountRequest obj, string sharedSecretKey)
        {
            string stringForSigning =
                obj.Username +
                obj.GiveawaysProvider;

            return ComputeHmacToken(stringForSigning, sharedSecretKey);
        }
    }
}
