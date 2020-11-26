using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MWM.API.Site.Service.Application.Common;
using MWM.API.Site.Service.Application.Link;
using MWM.Extensions.Authentication.JWT;
using System.Threading.Tasks;

namespace MWM.API.Site.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LinkController : ControllerBase
    {
        private readonly ILinkService _service;
        private readonly ILogger<LinkController> _logger;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IOptions<AppSettings> _settings;
        private readonly IGetJwtTokenInfoService _jwt;

        public LinkController(ILinkService service, ILogger<LinkController> logger, IHttpContextAccessor contextAccessor, IOptions<AppSettings> appSettings, IGetJwtTokenInfoService jwtuserInfo)
        {
            _service = service;
            _logger = logger;
            _contextAccessor = contextAccessor;
            _settings = appSettings;
            _jwt = jwtuserInfo;
        }

        /// <summary>
        /// 取得該帳號所有友情連結設定
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        public async Task<IActionResult> GetAll()
        {
            var id = _jwt.UserId;
            var result = await _service.GetAll(id);
            return Ok(result);
        }
        /// <summary>
        /// 依ID取得友情連結
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetLinkById(long id )
        {   
            var result = await _service.GetById(id);
            return Ok(result);
        }

       /// <summary>
       /// 新增友情連結
       /// </summary>
       /// <param name="request"></param>
       /// <returns></returns>
        [HttpPost()]
        public async Task<IActionResult> CreateLink(CreateUpdateLinkRequest request)
        {
            request.account_id = _jwt.UserId;
            if (request.account_id == 0) { return Content("401 without token"); }
            var result = await _service.CreateAsync(request);
            return Ok(result);
        }

        /// <summary>
        /// 更新友情連結
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut()]
        public async Task<IActionResult> UpdateLink(CreateUpdateLinkRequest request)
        {
            request.account_id = _jwt.UserId;
            var result = await _service.UpdateAsync(request);
            return Ok(result);
        }

        /// <summary>
        /// 依ID刪除友情連結
        /// </summary>
        /// <param name="id"></param>
      /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLink(long id)
        {
            var result = await _service.DeleteAsync(id, _jwt.UserId);
            return Ok(result);
        }
    }
}
