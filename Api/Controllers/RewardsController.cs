using System;

using Microsoft.AspNetCore.Mvc;
using NuciAPI.Responses;
using SteamGiveawaysBot.Server.Api.Models;
using SteamGiveawaysBot.Server.Service;

namespace SteamGiveawaysBot.Server.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RewardsController(IRewardService service) : ControllerBase
    {
        readonly IRewardService service = service;

        [HttpGet]
        public ActionResult GetAccount() => Ok();

        [HttpPost]
        public ActionResult RecordReward(
            [FromQuery] string username,
            [FromQuery] string gaProvider,
            [FromQuery] string gaId,
            [FromQuery] string steamUsername,
            [FromQuery] string steamAppId,
            [FromQuery] string activationKey,
            [FromQuery] string hmac)
        {
            try
            {
                RecordRewardRequest request = new()
                {
                    Username = username,
                    GiveawaysProvider = gaProvider,
                    GiveawayId = gaId,
                    SteamUsername = steamUsername,
                    SteamAppId = steamAppId,
                    ActivationKey = activationKey,
                    HmacToken = hmac
                };

                service.RecordReward(request);

                return Ok(SuccessResponse.Default);
            }
            catch (Exception ex)
            {
                return BadRequest(ErrorResponse.FromException(ex));
            }
        }
    }
}
