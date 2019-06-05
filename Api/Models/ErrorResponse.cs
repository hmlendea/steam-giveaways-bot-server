using System;

namespace SteamGiveawaysBot.Server.Api.Models
{
    public sealed class ErrorResponse : Response
    {
        public override bool IsSuccess => false;

        public string ErrorMessage { get; }

        public ErrorResponse(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        public ErrorResponse(Exception exception)
        {
            ErrorMessage = exception.Message;
        }
    }
}
