using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MWM.API.Site.Service.Application.Domain;
using MWM.API.Site.Service.Application.Site;
using MWM.API.Site.Service.Application.Site.Datatype;
using MWM.Extensions.Authentication.JWT;

using System;
using System.Threading.Tasks;

namespace MWM.API.Site.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SiteController : Controller
    {
        private readonly IGetJwtTokenInfoService _jwt;
        private readonly ILogger<SiteController> _logger;
        private readonly ISiteService _siteService;
        private readonly IDomainService _domainService;

        public SiteController(ILogger<SiteController> logger, 
                              ISiteService siteService, 
                              IDomainService domainService, 
                              IGetJwtTokenInfoService jwt)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _siteService = siteService ?? throw new ArgumentNullException(nameof(siteService));
            _domainService = domainService ?? throw new ArgumentNullException(nameof(domainService));
            _jwt = jwt ?? throw new ArgumentNullException(nameof(jwt));
        }



        private int FakeId()
        {
            int accountid;
            accountid = 123321456;
            return accountid;
        }
        /// <summary>
        /// 建立新的全站設定
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateSite(WebDetailRequest request)
        {
            var accountid = _jwt.UserId;
            var result = await _siteService.CreateWebsite(request,accountid);
            return Ok(result);
        }
        /// <summary>
        /// 取得所有站點設定
        /// </summary>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        [HttpGet("{page}/{rows}")]
        public async Task<IActionResult> GetAllSite(int page, int rows)
        {
            var result = await _siteService.GetWebsite(page, rows);
            return Ok(result);
        }
        /// <summary>
        /// 取得公司站點設定
        /// </summary>
        /// <param name="manager_id"></param>
        /// <returns></returns>
        [HttpGet("company")]
        public async Task<IActionResult> GetCompanySiteData(long manager_id)
        {
            var result = await _siteService.GetCompanySiteData(manager_id);
            return Ok(result);
        }

        /// <summary>
        /// 取得全站設定by流量主ID
        /// </summary>
        /// <returns></returns>
        [HttpGet("self")]
        public async Task<IActionResult> GetSiteByID()
        {   var accountid = _jwt.UserId;
           // accountid = FakeId();
            var result = await _siteService.GetWebsiteById(accountid);
            return Ok(result);
        }
        /// <summary>
        /// 刪除全站設定By流量主Id
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteSiteByID()
        {
            var accountid = _jwt.UserId;
          //  accountid = FakeId();
            var result = await _siteService.DeleteWebsite(accountid);
            return Ok(result);
        }
        /// <summary>
        /// 更新全站設定
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> UpdateSiteInfo(WebDetailRequest request)
        {   
            //request.account_id = FakeId();
            var result = await _siteService.UpdateWebsite(request);
            return Ok(result);
        }
    }
}