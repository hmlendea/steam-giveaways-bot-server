using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using SteamGiveawaysBot.Server.Api.Models;
using SteamGiveawaysBot.Server.Core.Extensions;
using SteamGiveawaysBot.Server.Service;

//using NuciLog.Core;

namespace SteamGiveawaysBot.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RewardsController : ControllerBase
    {
        readonly IRewardService service;

        public RewardsController(IRewardService service)
        {
            this.service = service;
        }
        [HttpGet]
        public ActionResult GetAccount()
        {
            return Ok();
        }

        [HttpPost]
        public ActionResult RecordReward(
            [FromQuery] string username,
            [FromQuery] string gaProvider,
            [FromQuery] string gaId,
            [FromQuery] string steamUsername,
            [FromQuery] string steamAppId,
            [FromQuery] string gameTitle,
            [FromQuery] string activationKey,
            [FromQuery] string hmac)
        {
            try
            {
                RecordRewardRequest request = new RecordRewardRequest
                {
                    Username = username,
                    GiveawaysProvider = gaProvider,
                    GiveawayId = gaId,
                    SteamUsername = steamUsername,
                    SteamAppId = steamAppId,
                    GameTitle = gameTitle,
                    ActivationKey = activationKey,
                    HmacToken = hmac
                };

                service.RecordReward(request);

                return Ok();
            }
            catch (Exception ex)
            {
                ErrorResponse response = new ErrorResponse(ex);
                return BadRequest(response);
            }
        }
    }
}
