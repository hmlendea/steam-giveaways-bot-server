using System;

namespace SteamGiveawaysBot.Server.Api.Models
{
    public abstract class Request
    {
        public string HmacToken { get; set; }
    }
}
