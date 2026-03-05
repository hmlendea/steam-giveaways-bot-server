using Microsoft.AspNetCore.Mvc;
using NuciAPI.Controllers;
using SteamGiveawaysBot.Server.Api.Models;
using SteamGiveawaysBot.Server.Service;

namespace SteamGiveawaysBot.Server.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsersController(IUserService service) : NuciApiController
    {
        readonly NuciApiAuthorisation authorisation = NuciApiAuthorisation.None;

        [HttpPost("ip/{username}")]
        public ActionResult RecordReward(
            [FromBody] SetIpAddressRequest request,
            string username)
            => ProcessRequest(
                request,
                () =>
                {
                    request.Username = username;
                    service.SetIpAddress(request);
                },
                authorisation);
    }
}
