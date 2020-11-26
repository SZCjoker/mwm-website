using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MWM.API.Account.Service.Application.Common;
using MWM.API.Account.Service.Application.Message;
using MWM.API.Account.Service.Application.Message.Contract;
using MWM.Extensions.Authentication.JWT;
using System.Threading.Tasks;

namespace MWM.API.Account.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _service;
        private readonly ILogger<MessageController> _logger;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IOptions<AppSettings> _settings;
        private readonly IGetJwtTokenInfoService _jwt;

        public MessageController(IMessageService service, ILogger<MessageController> logger, IHttpContextAccessor contextAccessor, IOptions<AppSettings> appSettings, IGetJwtTokenInfoService jwtuserInfo)
        {
            _service = service;
            _logger = logger;
            _contextAccessor = contextAccessor;
            _settings = appSettings;
            _jwt = jwtuserInfo;
        }

        /// <summary>
        /// 取得所有客服訊息
        /// </summary>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        [HttpGet()]
        public async Task<IActionResult> GetAll(int page , int rows )
        {
            var result = await _service.GetAll(page, rows);
            return Ok(result);
        }

        /// <summary>
        /// 依問題取得歷程( 客服中心1-1 )
        /// </summary>
        /// <param name="topic_id"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        [HttpGet("topic")]
        public async Task<IActionResult> GetByTopicId(long topic_id, int page , int rows)
        {
            var result = await _service.GetDetailByTopic(topic_id, page, rows);
            return Ok(result);
        }

        /// <summary>
        /// 依商戶取得訊息列表-標題及標題ID
        /// </summary>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        [HttpGet("merchant")]
        public async Task<IActionResult> GetByMerchantId(int page , int rows )
        {   var id = _jwt.UserId;
            var result = await _service.GetAllForMerchant(id, page, rows);
            return Ok(result);
        }

        /// <summary>
        /// 依照搜尋條件取得訊息列表
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        [HttpGet("query")]
        public async Task<IActionResult>QueryByCondition([FromQuery]QueryCondition condition)
        {            
            return Ok( await _service.QueryByCondition(condition));
        }
               
        /// <summary>
        ///  客服訊息建立/回復
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost()]
        public async Task<IActionResult> Create(CreateUpdateMessageRequest request)
        {
            request.login_name = _jwt.LoginName;
            request.account_id = _jwt.UserId;
            var result = await _service.CreateAsync(request);
            return Ok(result);
        }

        /// <summary>
        /// 編輯客服訊息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut()]
        public async Task<IActionResult> Update(CreateUpdateMessageRequest request)
        {
            request.account_id = _jwt.UserId;
            var result = await _service.UpdateAsync(request);
            return Ok(result);
        }

        /// <summary>
        ///  刪除客服訊息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _service.DeleteAsync(id, _jwt.UserId);
            return Ok(result);
        }
        /// <summary>
        /// 更改訊息狀態
        /// </summary>
        /// <param name="topic_id">TOPIC ID</param>
        /// <param name="state">0:新訊息,1:客服(管理員)已讀,2:使用者(流量主)已讀,3客服新訊息(管理員回覆) ,99:無效</param>
        /// <returns></returns>
        [HttpPut("readstate")]
        public async Task<IActionResult> ReadState(long topic_id ,int state)
        {   
            var result = await _service.ReadState(topic_id,state);
            return Ok(result);
        }
    }
}
