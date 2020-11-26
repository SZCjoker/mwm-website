using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MWM.API.Account.Service.Application.AccountTraffic;
using MWM.API.Account.Service.Application.AccountTraffic.Contract;
using MWM.API.Account.Service.Application.Common;
using MWM.Extensions.Authentication.JWT;
using System.Threading.Tasks;

namespace MWM.API.Account.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountTrafficController : ControllerBase
    {

        private readonly IAccountTrafficService _accountService;
        private readonly ILogger<AccountTrafficController> _logger;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IOptions<AppSettings> _settings;
        private readonly IGetJwtTokenInfoService _jwtuserInfo;


        public AccountTrafficController(IAccountTrafficService service, ILogger<AccountTrafficController> logger, IHttpContextAccessor contextAccessor, IOptions<AppSettings> appSettings, IGetJwtTokenInfoService jwtuserInfo)
        {
            _accountService = service;
            _logger = logger;
            _contextAccessor = contextAccessor;
            _settings = appSettings;
            _jwtuserInfo = jwtuserInfo;
        }

        [HttpGet]
        public async ValueTask<IActionResult> Get()
        {
            return Ok("OK");
        }

        /// <summary>
        /// 流量主Login
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("login")]
        public async ValueTask<IActionResult> LoginTraffic([FromBody] LoginTrafficRequest request)
        {
            request.login_ip = _contextAccessor.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            var result = await _accountService.LoginAsync(request);
            return Ok(result);

        }

        /// <summary>
        /// 建立流量主帳號
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost()]
        public async ValueTask<IActionResult> CreateTraffic([FromBody] TrafficRequest request)
        {
            var result = await _accountService.CreateAsync(request);
            return Ok(result);
        }

        /// <summary>
        /// 取得流量主帳號資訊
        /// </summary>
        /// <param name="account_id"></param>
        /// <returns></returns>
        [HttpGet("info")]
        public async ValueTask<IActionResult> GetTrafficInfo(int account_id)
        {
            account_id = account_id == 0 ? _jwtuserInfo.UserId : account_id;
            var result = await _accountService.GetInfoAsync(account_id);
            return Ok(result);
        }

        /// <summary>
        /// 更新流量主帳號
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut()]
        public async ValueTask<IActionResult> UpdateTraffic(TrafficUpdateRequest request)
        {
            request.account_id = request.account_id == 0 ? _jwtuserInfo.UserId : request.account_id;
            var result = await _accountService.UpdateAsync(request);
            return Ok(result);
        }

        /// <summary>
        /// 修改流量主密碼
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("update_password")]
        public async ValueTask<IActionResult> UpdateTrafficPassword(UpdateTrafficPasswordRequest request)
        {
            request.account_id = request.account_id == 0 ? _jwtuserInfo.UserId : request.account_id;
            var result = await _accountService.UpdatePasswordAsync(request);
            return Ok(result);
        }

        /// <summary>
        /// 流量主提出忘記密碼請求
        /// </summary>
        /// <param name="account_id"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPut("forget_password")]
        public async ValueTask<IActionResult> ForgetPassword(string account, string mail, string domain)
        {
            var result = await _accountService.ForgetPasswordAsync(account, mail, domain);
            return Ok(result);
        }

        /// <summary>
        /// 流量主忘記密碼重置
        /// </summary>
        /// <param name="token"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPut("forget_reset")]
        public async ValueTask<IActionResult> ForgetPasswordReset(ResetPasswordRequest request)
        {
            var result = await _accountService.ForgetPasswordResetAsync(request);
            return Ok(result);
        }


        /// <summary>
        /// 重置流量主密碼
        /// </summary>
        /// <param name="account_id"></param>
        /// <returns></returns>
        [HttpPut("reset_password")]
        public async ValueTask<IActionResult> ResetTrafficPassword(int account_id)
        {
            var result = await _accountService.ResetPasswordAsync(account_id);
            return Ok(result);
        }
    }
}