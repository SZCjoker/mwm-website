using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MWM.API.Site.Service.Application.AccountHistory;
using MWM.Extensions.Authentication.JWT;

namespace MWM.API.Site.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountHistoryController : Controller
    {

        private readonly IAccountHistoryService _service;
        private readonly ILogger<AccountHistoryController> _logger;
        private readonly IGetJwtTokenInfoService _jwt;


        public AccountHistoryController(IAccountHistoryService service, 
                                        ILogger<AccountHistoryController> logger, 
                                        IGetJwtTokenInfoService jwt)
        {
            _logger = logger;
            _service = service;
            _jwt = jwt;
        }


        /// <summary>
        /// 取得使用者行為紀錄ByCondition
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("GetAccountHistory")]
        public async Task<IActionResult> GetAccountHistoryByCondition([FromQuery]QueryRequest request)
        {
            var data = await _service.GetAccountHistoryByCondition(request);
            return Ok(data);
        }
        /// <summary>
        /// 取得所有使用者行為紀錄
        /// </summary>
        /// <param ></param>
        /// <returns></returns>
        [HttpGet("GetAllHistory")]
        public async Task<IActionResult> GetAccountHistory()
        {
            var data = await _service.GetAllHistory();
            return Ok(data);
        }
        /// <summary>
        /// 行為紀錄
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("Record")]
        public async Task<IActionResult> Record(HistoryRequest request)
        {
            request.account_id = _jwt.UserId;
            _logger.LogInformation($"accountid{request.account_id}--");
            _logger.LogInformation($"contro_name{request.control_name}--");
            _logger.LogInformation($"restful_method{request.restful_method}--");
            _logger.LogInformation($"category{request.category}--");         
            var data = await _service.SaveAccountHistory(request);
            return Ok(data);
        }
    }
}