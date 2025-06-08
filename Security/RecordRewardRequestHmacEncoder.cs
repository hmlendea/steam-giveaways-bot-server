using NuciSecurity.HMAC;

using SteamGiveawaysBot.Server.Api.Models;

namespace SteamGiveawaysBot.Server.Security
{
    public sealed class RecordRewardRequestHmacEncoder : HmacEncoder<RecordRewardRequest>
    {
        public override string GenerateToken(RecordRewardRequest obj, string sharedSecretKey)
            => ComputeHmacToken(
                obj.Username +
                obj.GiveawaysProvider +
                obj.GiveawayId +
                obj.SteamUsername +
                obj.SteamAppId +
                obj.ActivationKey,
                sharedSecretKey);
    }
}
