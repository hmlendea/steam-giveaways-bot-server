using Microsoft.AspNetCore.Mvc;

using NuciAPI.Controllers;

using SteamGiveawaysBot.Server.Configuration;
using SteamGiveawaysBot.Server.Requests;
using SteamGiveawaysBot.Server.Service;

namespace SteamGiveawaysBot.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SteamAccountController(
        ISteamAccountService service,
        SecuritySettings securitySettings) : NuciApiController
    {
        [HttpGet("{username}")]
        public ActionResult GetAccount(
            [FromQuery] GetSteamAccountRequest request,
            string username)
            => ProcessRequest(
                request,
                () =>
                {
                    request.Username = username;

                    return service.GetAccount(request);
                },
                NuciApiAuthorisation.ApiKey(securitySettings.ApiKey));
    }
}
