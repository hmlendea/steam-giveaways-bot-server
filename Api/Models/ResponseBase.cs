using System;

namespace SteamGiveawaysBot.Server.Api.Models
{
    public abstract class ResponseBase
    {
        public abstract bool IsSuccess { get; }
    }
}
