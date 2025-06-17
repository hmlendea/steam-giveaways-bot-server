using System;
using Microsoft.AspNetCore.Mvc;
using NuciAPI.Responses;
using SteamGiveawaysBot.Server.Api.Models;
using SteamGiveawaysBot.Server.Service;

namespace SteamGiveawaysBot.Server.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsersController(IUserService service) : ControllerBase
    {
        [HttpPost("ip/{username}")]
        public ActionResult SetIpAddress(
            [FromBody] SetIpAddressRequest request,
            string username)
        {
            if (request is null)
            {
                return BadRequest(ErrorResponse.InvalidRequest);
            }

            request.Username = username;

            try
            {
                service.SetIpAddress(request);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ErrorResponse.FromException(ex));
            }
        }
    }
}
