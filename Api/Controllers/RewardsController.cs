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
        public ActionResult RecordReward([FromBody] RecordRewardRequest request)
        {
            if (request is null)
            {
                return BadRequest(ErrorResponse.InvalidRequest);
            }

            try
            {
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
