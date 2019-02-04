using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using SteamAccountDistributor.Api.Mapping;
using SteamAccountDistributor.Api.Models;
using SteamAccountDistributor.Core.Extensions;
using SteamAccountDistributor.DataAccess.DataObjects;
using SteamAccountDistributor.DataAccess.Repositories;

//using NuciLog.Core;

namespace SteamAccountDistributor.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SteamAccountController : ControllerBase
    {
        readonly IUserRepository userRepository;
        readonly ISteamAccountRepository steamAccountRepository;
        //readonly INuciLogger logger;

        public SteamAccountController(
            IUserRepository userRepository,
            ISteamAccountRepository steamAccountRepository)//,
            //INuciLogger logger)
        {
            this.userRepository = userRepository;
            this.steamAccountRepository = steamAccountRepository;
            //this.logger = logger;
        }

        [HttpGet]
        public ActionResult<IEnumerable<SteamAccount>> Get()
        {
            IEnumerable<SteamAccount> steamAccounts = steamAccountRepository.GetAll().ToApiModels();

            return Ok(steamAccounts);
        }

        [HttpGet("{username}")]
        public ActionResult<SteamAccount> Get(string username)
        {
            User user = userRepository.Get(username).ToApiModel();

            if (string.IsNullOrWhiteSpace(user.AssignedSteamAccount))
            {
                // TODO: Make sure that the new account is not already assigned to some other user
                user.AssignedSteamAccount = steamAccountRepository.GetAll().GetRandomElement().Username;
            }

            SteamAccount steamAccount = steamAccountRepository.Get(user.AssignedSteamAccount).ToApiModel();

            return Ok(steamAccount);
        }
    }
}
