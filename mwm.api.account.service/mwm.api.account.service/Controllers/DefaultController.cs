using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MWM.Extensions.Authentication.JWT;
using System;
using System.Threading.Tasks;

namespace MWM.API.Account.Service.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    [ApiController]
    public class DefaultController : ControllerBase
    {
        private readonly ILogger<DefaultController> _logger;
        private readonly IGetJwtTokenInfoService _jwtTokenInfo;
        public DefaultController(ILogger<DefaultController> logger, IGetJwtTokenInfoService jwtTokenInfo)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _jwtTokenInfo = jwtTokenInfo;
        }
        /// <summary>
        /// default 
        /// </summary>
        /// <returns></returns>
      //[AllowAnonymous]
        [Authorize]
        [HttpGet]
        public ValueTask<string> Get()
        {   
             var userId=    _jwtTokenInfo.UserId;
            return new ValueTask<string>($"{DateTimeOffset.UtcNow}:{Environment.MachineName}");
        }
    }
}