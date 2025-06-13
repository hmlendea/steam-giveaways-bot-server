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
            string username,
            [FromQuery] string ip,
            [FromQuery] string hmac)
        {
            SetIpAddressRequest request = new()
            {
                Username = username,
                IpAddress = ip,
                HmacToken = hmac
            };

            service.SetIpAddress(request);

            return Ok(SuccessResponse.Default);
        }
    }
}
