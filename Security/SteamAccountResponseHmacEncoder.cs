using SteamGiveawaysBot.Server.Api.Models;

namespace SteamGiveawaysBot.Server.Security
{
    public sealed class SteamAccountResponseHmacEncoder : HmacEncoder<SteamAccountResponse>
    {
        public override string GenerateToken(SteamAccountResponse obj, string sharedSecretKey)
        {
            string stringForSigning =
                obj.Username +
                obj.Password +
                obj.IsSuccess;

            return ComputeHmacToken(stringForSigning, sharedSecretKey);
        }
    }
}
