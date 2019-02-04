using System;

namespace SteamAccountDistributor.Api.Models
{
    public class SuccessResponse : ResponseBase
    {
        public override bool IsSuccess => true;
    }
}
