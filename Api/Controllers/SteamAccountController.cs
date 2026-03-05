using Microsoft.AspNetCore.Mvc;
using NuciAPI.Controllers;
using SteamGiveawaysBot.Server.Api.Models;
using SteamGiveawaysBot.Server.Service;

namespace SteamGiveawaysBot.Server.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SteamAccountController(ISteamAccountService service) : NuciApiController
    {
        readonly NuciApiAuthorisation authorisation = NuciApiAuthorisation.None;

        [HttpGet("{username}")]
        public ActionResult GetAccount(
            [FromBody] SteamAccountRequest request,
            string username)
            => ProcessRequest(
                request,
                () =>
                {
                    request.Username = username;
                    return service.GetAccount(request);
                },
                authorisation);
    }
}
