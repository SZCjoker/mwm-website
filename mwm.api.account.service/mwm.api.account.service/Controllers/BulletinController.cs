 using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MWM.API.Account.Service.Application.Bulletin;
using MWM.API.Account.Service.Application.Bulletin.Contract;
using MWM.API.Account.Service.Application.Common;
using MWM.Extensions.Authentication.JWT;
using System.Threading.Tasks;

namespace MWM.API.Account.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BulletinController : ControllerBase
    {
        private readonly IBulletinService _service;
        private readonly ILogger<BulletinController> _logger;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IOptions<AppSettings> _settings;
        private readonly IGetJwtTokenInfoService _jwt;

        public BulletinController(IBulletinService service, ILogger<BulletinController> logger, IHttpContextAccessor contextAccessor, IOptions<AppSettings> appSettings, IGetJwtTokenInfoService jwtuserInfo)
        {
            _service = service;
            _logger = logger;
            _contextAccessor = contextAccessor;
            _settings = appSettings;
            _jwt = jwtuserInfo;
        }
       
        /// <summary>
        /// 取得公告消息列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        [HttpGet()]
        public async Task<IActionResult> GetAll(int page = 1, int rows = 10)
        {
            var result = await _service.GetAll(page, rows);
            return Ok(result);
        }
        /// <summary>
        /// 取得公告消息ByID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _service.GetById(id);
            return Ok(result);
        }
        /// <summary>
        /// 建立公告訊息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost()]
        [RequestSizeLimit(1000000)]
        public async Task<IActionResult> Create(CreateUpdateBulletinRequest request)
        {
            var accountid = _jwt.UserId;
            var loginname = _jwt.LoginName;
            var result = await _service.CreateAsync(request,accountid,loginname);
            return Ok(result);
        }
        /// <summary>
        /// 編輯公告訊息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut()]
        [RequestSizeLimit(1000000)]
        public async Task<IActionResult> Update(CreateUpdateBulletinRequest request)
        {   
            var result = await _service.UpdateAsync(request);
            return Ok(result);
        }
        /// <summary>
        /// 刪除公告訊息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _service.DeleteAsync(id);
            return Ok(result);
        }

        [HttpGet("query")]
        public async Task<IActionResult> Query(string title,int page,int rows)
        {
            var result = await _service.Query(title,page,rows);
            return Ok(result);
        }

    }
}
