using System;

namespace SteamAccountDistributor.Api.Models
{
    public abstract class ResponseBase
    {
        public abstract bool IsSuccess { get; }
    }
}
