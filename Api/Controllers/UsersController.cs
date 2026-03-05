using Microsoft.AspNetCore.Mvc;
using NuciAPI.Controllers;
using SteamGiveawaysBot.Server.Api.Models;
using SteamGiveawaysBot.Server.Configuration;
using SteamGiveawaysBot.Server.Service;

namespace SteamGiveawaysBot.Server.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsersController(
        IUserService service,
        SecuritySettings securitySettings) : NuciApiController
    {
        readonly NuciApiAuthorisation authorisation = NuciApiAuthorisation.ApiKey(securitySettings.ApiKey);

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
