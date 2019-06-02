using SteamGiveawaysBot.Server.Api.Models;

namespace SteamGiveawaysBot.Server.Security
{
    public sealed class RecordRewardRequestHmacEncoder : HmacEncoder<RecordRewardRequest>
    {
        public override string GenerateToken(RecordRewardRequest obj, string sharedSecretKey)
        {
            string stringForSigning =
                obj.Username +
                obj.GiveawaysProvider +
                obj.GiveawayId +
                obj.SteamUsername +
                obj.SteamAppId +
                obj.GameTitle +
                obj.ActivationKey;

            string hmacToken = ComputeHmacToken(stringForSigning, sharedSecretKey);

            return hmacToken;
        }
    }
}
