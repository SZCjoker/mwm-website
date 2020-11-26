using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MWM.API.Site.Service.Application.Code;
using MWM.API.Site.Service.Application.Code.Contract;
using MWM.API.Site.Service.Application.Common;
using MWM.Extensions.Authentication.JWT;
using System.Threading.Tasks;

namespace MWM.API.Site.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CodeController : ControllerBase
    {
        private readonly ICodeService _service;
        private readonly ILogger<CodeController> _logger;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IOptions<AppSettings> _settings;
        private readonly IGetJwtTokenInfoService _jwt;

        public CodeController(ICodeService service, ILogger<CodeController> logger, IHttpContextAccessor contextAccessor, IOptions<AppSettings> appSettings, IGetJwtTokenInfoService jwtuserInfo)
        {
            _service = service;
            _logger = logger;
            _contextAccessor = contextAccessor;
            _settings = appSettings;
            _jwt = jwtuserInfo;
        }

        /// <summary>
        /// 取得所有用戶流量代碼設定
        /// </summary>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        [HttpGet("all")]
        public async Task<IActionResult> GetALL(int page, int rows)
        {
            var result = await _service.GetALL(page, rows); ;
            return Ok(result);
        }
        /// <summary>
        /// MWM模板取得流量code
        /// </summary>
        /// <returns></returns>
        [HttpGet("template/account_id")]
        public async Task<IActionResult> GetTemplateCdoeByAccount(long account_id)
        {
            var result = await _service.GetCodeByAccount(account_id);
            
            return Ok(result);
        }
        /// <summary>
        /// 流量主-依用戶取得流量代碼設定-token
        /// </summary>
        /// <returns></returns>
        [HttpGet("account")]
        public async Task<IActionResult> GetCodeByAccount()
        {
            var accountid = _jwt.UserId;
            var result = await _service.GetCodeByAccount(accountid);
            return Ok(result);
        }
        /// <summary>
        /// 取得公版流量代碼
        /// </summary>
        /// <param name="mananger_id"></param>
        /// <returns></returns>
        [HttpGet("company")]
       public async Task<IActionResult> GetCompanyCode(long mananger_id)
        {
            var result = await _service.GetCompanyCode(mananger_id);
            return Ok(result);
        }

        /// <summary>
        /// 新增流量代碼
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost()]
        public async Task<IActionResult> CreateCode(CreateUpdateCodeRequest request)
        {
            request.account_id = _jwt.UserId;
            var result = await _service.CreateAsync(request);
            return Ok(result);
        }
        /// <summary>
        /// 依帳戶更新流量代碼
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut()]
        public async Task<IActionResult> UpdateCode(CreateUpdateCodeRequest request)
        {
            request.account_id = _jwt.UserId;
            var result = await _service.UpdateAsync(request);
            return Ok(result);
        }

        /// <summary>
        /// 依帳戶刪除流量代碼
        /// </summary>
        /// <returns></returns>
        [HttpDelete()]
        public async Task<IActionResult> DeleteCode()
        {
            var result = await _service.DeleteAsync(_jwt.UserId);
            return Ok(result);
        }
    }
}
