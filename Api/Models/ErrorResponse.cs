using System;

namespace SteamAccountDistributor.Api.Models
{
    public sealed class ErrorResponse : ResponseBase
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
