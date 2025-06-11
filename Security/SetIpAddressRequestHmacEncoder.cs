using NuciSecurity.HMAC;

using SteamGiveawaysBot.Server.Api.Models;

namespace SteamGiveawaysBot.Server.Security
{
    public sealed class SetIpAddressRequestHmacEncoder : HmacEncoder<SetIpAddressRequest>
    {
        public override string GenerateToken(SetIpAddressRequest obj, string sharedSecretKey)
            => ComputeHmacToken(obj.Username + obj.IpAddress, sharedSecretKey);
    }
}
