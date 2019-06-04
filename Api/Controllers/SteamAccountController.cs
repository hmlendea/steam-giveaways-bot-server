using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using SteamGiveawaysBot.Server.Api.Models;
using SteamGiveawaysBot.Server.Service;

namespace SteamGiveawaysBot.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SteamAccountController : ControllerBase
    {
        readonly ISteamAccountService service;

        public SteamAccountController(ISteamAccountService service)
        {
            this.service = service;
        }

        [HttpGet("{username}")]
        public ActionResult<SteamAccountResponse> GetAccount(
            string username,
            [FromQuery] string gaProvider,
            [FromQuery] string hmac)
        {
            try
            {
                SteamAccountRequest request = new SteamAccountRequest
                {
                    Username = username,
                    GiveawaysProvider = gaProvider,
                    HmacToken = hmac
                };

                return service.GetAccount(request);
            }
            catch (Exception ex)
            {
                ErrorResponse response = new ErrorResponse(ex);
                return BadRequest(response);
            }
        }
    }
}
