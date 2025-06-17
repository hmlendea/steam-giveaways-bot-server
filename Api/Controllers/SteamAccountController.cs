using System;

using Microsoft.AspNetCore.Mvc;
using NuciAPI.Responses;
using SteamGiveawaysBot.Server.Api.Models;
using SteamGiveawaysBot.Server.Service;

namespace SteamGiveawaysBot.Server.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SteamAccountController(ISteamAccountService service) : ControllerBase
    {
        readonly ISteamAccountService service = service;

        [HttpGet("{username}")]
        public ActionResult<SteamAccountResponse> GetAccount(
            [FromBody] SteamAccountRequest request,
            string username)
        {
            if (request is null)
            {
                return BadRequest(ErrorResponse.InvalidRequest);
            }

            request.Username = username;

            try
            {
                return Ok(service.GetAccount(request));
            }
            catch (Exception ex)
            {
                return BadRequest(ErrorResponse.FromException(ex));
            }
        }
    }
}
