using Microsoft.AspNetCore.Mvc;

using NuciAPI.Controllers;

using SteamGiveawaysBot.Server.Configuration;
using SteamGiveawaysBot.Server.Requests;
using SteamGiveawaysBot.Server.Service;

namespace SteamGiveawaysBot.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsersController(
        IUserService service,
        SecuritySettings securitySettings) : NuciApiController
    {
        [HttpPost("ip/{username}")]
        public ActionResult SetIpAddress(
            [FromBody] SetIpAddressRequest request,
            string username)
            => ProcessRequest(
                request,
                () =>
                {
                    request.Username = username;
                    service.SetIpAddress(request);
                },
                NuciApiAuthorisation.ApiKey(securitySettings.ApiKey));
    }
}
