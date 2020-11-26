using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MWM.API.Site.Service.Application.Common;
using MWM.API.Site.Service.Application.Domain;
using MWM.Extensions.Authentication.JWT;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace MWM.API.Site.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DomainController : Controller
    {
        private readonly ILogger<DomainController> _logger;
        private readonly IDomainService _service;
        private readonly IGetJwtTokenInfoService _jwt;
        private readonly IOptions<AppSettings> _setting;

        public DomainController(ILogger<DomainController> logger,
                                IDomainService service,
                                IGetJwtTokenInfoService jwt,
                                IOptions<AppSettings> setting)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _jwt = jwt ?? throw new ArgumentNullException(nameof(jwt));
            _setting = setting;
        }

        #region for video domain
        private int FakeId()
        {
            int accountid;
            accountid = 123321456;
            return accountid;
        }
        /// <summary>
        /// 建立影片瀏覽域名
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateDomain(VideoDomainRequest detail)
        {
            return Ok(await _service.CreateDomain(detail));
        }
        /// <summary>
        ///  取得所有影片瀏覽域名
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAllDomain()
        {
            return Ok(await _service.GetDomains());
        }

        /// <summary>
        /// 刪除影片瀏覽域名
        /// </summary>
        /// <param name="id">1044750336</param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DeteleDomainByID(int id)
        {
            return Ok(await _service.DeleteDomain(id));
        }
        /// <summary>
        /// 編輯影片瀏覽域名
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> UpdateDomainInfo([FromBody]List<VideoDomainRequest> request)
        {
            return Ok(await _service.UpdateDomain(request));
        }
        #endregion


        #region website domain
        /// <summary>
        /// 取得所有網站域名資料
        /// </summary>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        [HttpGet("site")]
        public async Task<IActionResult> GetAllWebSiteDomain(int page, int rows)
        {
            var result = await _service.GetSiteDomains(page, rows);
            return Ok(result);
        }
        /// <summary>
        /// 取得帳號所屬的網域
        /// </summary>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        [HttpGet("site/self")]
        public async Task<IActionResult> GetWebSiteDomainByID(int page, int rows)
        {
            var accountid = _jwt.UserId;
            //accountid = FakeId();
            return Ok(await _service.GetSiteDomainById(accountid, page, rows));
        }
        /// <summary>
        /// 建立新的網站域名
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>        
        [HttpPost("site")]
        public async Task<IActionResult> CreteWebSiteDomain(WebSiteDomainRequest request)
        {  
            var accountid = _jwt.UserId;
            return Ok(await _service.CreateSiteDomain(request, accountid));
        }
        /// <summary>
        /// 刪除(停權)網站域名
        /// </summary>
        /// <param name="domain_id"></param>
        /// <returns></returns>
        [HttpDelete("site")]
        public async Task<IActionResult> DeteleWebSiteDomainByID(long domain_id)
        {   
            //accountid = FakeId();
            return Ok(await _service.DeleteSiteDomain(domain_id));
        }
        /// <summary>
        /// 變更網站域名資訊
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("site")]
        public async Task<IActionResult> UpdateWebSiteDomain(WebSiteDomainRequest request)
        { 
            //request.account_id = FakeId();
            var result = await _service.UpdateSiteDomain(request);
            return Ok(result);
        }
        /// <summary>
        /// MWM模板獲取流量主網站設定LIST
        /// </summary>
        /// <param name="account_id">流量主ID</param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        [HttpGet("template/{account_id}")]
        public async Task<IActionResult> GetWebdomainSettingByAccount(long account_id)
        {
            var result = await _service.GetWebdomainSettingByAccount(account_id);
            return Ok(result);
        }

        #endregion


        #region for dispatch domain
        /// <summary>
        /// 取得所有系統分配給流量主的網站域名
        /// </summary>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        [HttpGet("dispatch")]
        public async Task<IActionResult> GetDispatchDomain(int page ,int rows)
        {
            var result = await _service.GetDispatchDomain(page,rows);
            return Ok(result);
        }
        /// <summary>
        /// 取得系統分配域名By流量主Id
        /// </summary>
        /// <returns></returns>
        [HttpGet("dispatch/detail")]
        public async Task<IActionResult> GetDispatchDomainNameByAccount()
        {
            var accountid = _jwt.UserId;
            //accountid = FakeId();
            var result = await _service.GetDispatchDomainNameByAccount(accountid);
            return Ok(result);
        }

        /// <summary>
        /// 建立系統配置三級域名
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("dispatch/create")]
        public async Task<IActionResult> CreateDispatchData(DispatchRequest request)
        {
            request.account_id = _jwt.UserId;
            var result = await _service.CreateDispatchData(request);
            return Ok(result);
        }
        #endregion

        
        



        //#region test
        //[HttpGet("test")]
        //public async Task<IActionResult> test()
        //{
        //    var defaultValue = _setting.Value.application.defaultvalue.link;
        //    var links = defaultValue.Split("|").ToList();
        //    var link = links.Select(d => d.Split(",")).ToList();
        //    var value = link.Select(d => d.SelectMany(s => s.Split(":"))).ToList();

        //    return Ok();
        //}
        //#endregion

    }
}