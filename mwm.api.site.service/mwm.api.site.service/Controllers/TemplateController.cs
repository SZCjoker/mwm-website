using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MWM.API.Site.Service.Application.Template;
using MWM.Extensions.Authentication.JWT;

using System;
using System.Threading.Tasks;

namespace MWM.API.Site.Service.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class TemplateController : Controller
    {
        private readonly ILogger<TemplateController> _logger;
        private readonly ITemplateService _service;
        private readonly IGetJwtTokenInfoService _jwtTokenInfoService;

        public TemplateController(ILogger<TemplateController> logger, ITemplateService service, IGetJwtTokenInfoService jwtTokenInfoService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _jwtTokenInfoService = jwtTokenInfoService ?? throw new ArgumentNullException(nameof(jwtTokenInfoService));
        }

       
        /// <summary>
        /// 新增網站模板
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateTemplate(TemplateRequest request)
        {
            var result = await _service.CreteTemplate(request);
            return Ok(result);
        }
        /// <summary>
        /// 取得所有網站模板
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAllTemplate()
        {
            var result = await _service.GetTemplates();
            return Ok(result);
        }
        /// <summary>
        /// 取得網站模板By網站Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTemplateByID(int id)
        {
            var result = await _service.GetTemplateById(id);
            return Ok(result);
        }
        /// <summary>
        /// 刪除網站模板ById
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeteleTemplateByID(int id)
        {
            var result = await _service.DeleteTemplate(id);
            return Ok(result);
        }
        /// <summary>
        /// 更新維護網站模板
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> UpdateTemplate(TemplateRequest request)
        {
            var result = await _service.UpdateTemplate(request);
            return Ok(result);
        }
        [HttpGet("query")]
        public async Task<IActionResult> Query([FromQuery]QueryRequest request)
        {
            var result = await _service.QureybyCondition(request);
            return Ok(result);
        }

    }
}