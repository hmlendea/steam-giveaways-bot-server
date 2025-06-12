using System;

using Microsoft.AspNetCore.Mvc;
using NuciAPI.Responses;
using SteamGiveawaysBot.Server.Api.Models;
using SteamGiveawaysBot.Server.Service;

namespace SteamGiveawaysBot.Server.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SteamAccountController(ISteamAccountService service) : ControllerBase
    {
        readonly ISteamAccountService service = service;

        [HttpGet("{username}")]
        public ActionResult<SteamAccountResponse> GetAccount(
            string username,
            [FromQuery] string gaProvider,
            [FromQuery] string hmac)
        {
            try
            {
                SteamAccountRequest request = new()
                {
                    Username = username,
                    GiveawaysProvider = gaProvider,
                    HmacToken = hmac
                };

                return service.GetAccount(request);
            }
            catch (Exception ex)
            {
                return BadRequest(ErrorResponse.FromException(ex));
            }
        }
    }
}
