using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace MWM.API.Site.Service.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    [ApiController]
    public class DefaultController : ControllerBase
    {
        private readonly ILogger<DefaultController> _logger;

        public DefaultController(ILogger<DefaultController> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        /// <summary>
        /// heartbeat
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public ValueTask<string> Get()
        {
            throw new Exception();
            return new ValueTask<string>($"{DateTimeOffset.UtcNow}:{Environment.MachineName}");
        }
    }
}