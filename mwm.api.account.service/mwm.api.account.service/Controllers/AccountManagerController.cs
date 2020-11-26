using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MWM.API.Account.Service.Application.AccountManager;
using MWM.API.Account.Service.Application.AccountManager.Contract;
using MWM.API.Account.Service.Application.AccountTraffic;
using MWM.API.Account.Service.Application.AccountTraffic.Contract;
using MWM.API.Account.Service.Application.Common;
using MWM.Extensions.Authentication.JWT;

namespace MWM.API.Account.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountManagerController : ControllerBase
    {
        private readonly IAccountTrafficService _trafficService;
        private readonly IAccountManagerService _service;
        private readonly ILogger<AccountManagerController> _logger;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IOptions<AppSettings> _settings;
        private readonly IGetJwtTokenInfoService _jwtuserInfo;


        public AccountManagerController(IAccountManagerService service, ILogger<AccountManagerController> logger, IHttpContextAccessor contextAccessor, IOptions<AppSettings> appSettings, IGetJwtTokenInfoService jwtuserInfo, IAccountTrafficService trafficService)
        {
            _service = service;
            _logger = logger;
            _contextAccessor = contextAccessor;
            _settings = appSettings;
            _jwtuserInfo = jwtuserInfo;
            _trafficService = trafficService;
        }

        /// <summary>
        /// 取得所有流量主帳號清單
        /// </summary>
        /// <param name="login_name"></param>
        /// <param name="state"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        [HttpGet("traffic")]
        public async ValueTask<IActionResult> GetTrafficAll(string login_name, int state, int page, int size)
        {
            var result = await _service.GetTrafficAccountPageAsync(login_name, state, page, size);
            return Ok(result);
        }

        /// <summary>
        /// Login
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public async ValueTask<IActionResult> Login(LoginManagerRequest request)
        {
            var result = await _service.LoginAsync(request);
            return Ok(result);
        }

        /// <summary>
        /// 取得指定帳號資訊
        /// </summary>
        /// <param name="account_id"></param>
        /// <returns></returns>
        [HttpGet("info")]
        public async ValueTask<IActionResult> GetManagerInfo(int account_id)
        {
            var result = await _service.GetManagerInfoAsync(account_id);
            return Ok(result);
        }

        /// <summary>
        /// 取得所有帳號資訊
        /// </summary>
        /// <returns></returns>
        [HttpPost("all")]
        public async ValueTask<IActionResult> GetManagerAll(ManagerListRequest request)
        {
            var result = await _service.GetManagerAllAsync(request);
            return Ok(result);
        }

        /// <summary>
        /// 建立帳號
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("")]
        public async ValueTask<IActionResult> CreateManagerAccount(ManagerRequest request)
        {
            var result = await _service.CreateAsync(request);
            return Ok(result);
        }

        /// <summary>
        /// 更新帳號
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("")]
        public async ValueTask<IActionResult> UpdateManagerAccount(ManagerRequest request)
        {
            var result = await _service.UpdateAsync(request);
            return Ok(result);
        }

        /// <summary>
        /// 刪除帳號
        /// </summary>
        /// <param name="account_id"></param>
        /// <returns></returns>
        [HttpDelete("")]
        public async ValueTask<IActionResult> DeleteManagerAccount(int account_id)
        {
            var state = 99;
            var result = await _service.ChangeStateAsync(account_id, state);
            return Ok(result);
        }
    }
}