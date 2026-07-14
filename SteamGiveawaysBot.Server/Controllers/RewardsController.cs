using Microsoft.AspNetCore.Mvc;
using NuciAPI.Controllers;
using SteamGiveawaysBot.Server.Configuration;
using SteamGiveawaysBot.Server.Requests;
using SteamGiveawaysBot.Server.Service;

namespace SteamGiveawaysBot.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RewardsController(
        IRewardService service,
        SecuritySettings securitySettings) : NuciApiController
    {
        readonly NuciApiAuthorisation authorisation = NuciApiAuthorisation.ApiKey(securitySettings.ApiKey);

        [HttpGet]
        public ActionResult GetAccount() => Ok();

        [HttpPost]
        public ActionResult RecordReward([FromBody] RecordRewardRequest request)
            => ProcessRequest(
                request,
                () => service.RecordReward(request),
                authorisation);
    }
}
