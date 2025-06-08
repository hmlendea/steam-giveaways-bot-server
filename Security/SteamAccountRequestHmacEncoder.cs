using NuciSecurity.HMAC;

using SteamGiveawaysBot.Server.Api.Models;

namespace SteamGiveawaysBot.Server.Security
{
    public sealed class SteamAccountRequestHmacEncoder : HmacEncoder<SteamAccountRequest>
    {
        public override string GenerateToken(SteamAccountRequest obj, string sharedSecretKey)
            => ComputeHmacToken(obj.Username + obj.GiveawaysProvider, sharedSecretKey);
    }
}
