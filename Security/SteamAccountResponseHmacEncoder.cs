using NuciSecurity.HMAC;

using SteamGiveawaysBot.Server.Api.Models;

namespace SteamGiveawaysBot.Server.Security
{
    public sealed class SteamAccountResponseHmacEncoder : HmacEncoder<SteamAccountResponse>
    {
        public override string GenerateToken(SteamAccountResponse obj, string sharedSecretKey)
            => ComputeHmacToken(obj.Username + obj.Password + obj.IsSuccess, sharedSecretKey);
    }
}
