using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MWM.API.Advert.Service.Application.Common;
using MWM.API.Advert.Service.Application.Receivable;
using MWM.API.Advert.Service.Application.Receivable.Contract;
using MWM.API.Advert.Service.Application.Receivable.Record;
using MWM.Extensions.Authentication.JWT;
using System;
using System.Threading.Tasks;

namespace MWM.API.Advert.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReceivableController : ControllerBase
    {
        private readonly IReceivableBankService _bankService;
        private readonly IReceivablePasswordService _passwordService;
        private readonly IReceivableRecordService _recordService;
        private readonly IHttpContextAccessor _context;
        private readonly IGetJwtTokenInfoService _jwt;
        private readonly IOptions<AppSettings> _settings;
        private readonly ILogger<ReceivableController> _logger;
         
        public ReceivableController(IReceivablePasswordService configService, IReceivableBankService bankService, IReceivableRecordService recordService, ILogger<ReceivableController> logger, IHttpContextAccessor context, IOptions<AppSettings> settings, IGetJwtTokenInfoService jwt)
        {
            _jwt = jwt;
            _logger = logger;            
            _bankService = bankService;
            _passwordService = configService;
            _recordService = recordService;
            _settings = settings;
            _context = context;
        }


        #region password
        /// <summary>
        /// 公司後台-取得所有密碼LIST 人工更改查詢可用
        /// </summary>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        [HttpGet("password/all")]
        public async Task<IActionResult> GetAllpassword(int page,int size)
        {
            var result =await _passwordService.GetAllPasswrod(page, size);
            return Ok(result);
        }

        /// <summary>
        /// 公司後台-依帳戶取得密碼
        /// </summary>
        /// <returns></returns>
        [HttpGet("password")]
        public async Task<IActionResult> GetPasswordByAccount(long account_id)
        {
            var result = await _passwordService.GetPwdByAccount(account_id);
            return Ok(result);
        }

        /// <summary>
        ///  公司後台-依ID,帳戶ID取得密碼
        /// </summary>
        /// <param name="id">流水號ID</param>
        /// <param name="account_id">流量主ID</param>
        /// <returns></returns>
        [HttpGet("password/{id}")]
        public async Task<IActionResult> GetPasswordById(long id,long account_id)
        {
            var result = await _passwordService.GetPwdById(id, account_id);
            return Ok(result);
        }
        /// <summary>
        /// 公司後台-重置流量主提現密碼
        /// </summary>
        /// <param name="request">流量主ID</param>
        /// <returns></returns>
        [HttpPut("reset")]
        public async Task<IActionResult> ResetPassword(long account_id)
        {
            var result = await _passwordService.ResetPassword(account_id);
            return Ok(result);
        }

        /// <summary>
        ///流量主-建立提現密碼
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("password")]
        public async Task<IActionResult> CreatePassword(CreatePasswordRequest request)
        {
            request.ip = _context.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            var accountid = _jwt.UserId;
            var result = await _passwordService.CreatePwdAsync(request,accountid);
            return Ok(result);
        }

        /// <summary>
        /// 流量主--修改提現密碼
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("password")]
        public async Task<IActionResult> UpdatePassword(UpdatePasswordRequest request)
        {  
            request.ip = _context.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            var accountid = _jwt.UserId;
            var result = await _passwordService.UpdatePwdAsync(request, accountid);
            return Ok(result);
        }
        /// <summary>
        /// 流量主--刪除密碼
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("password/{id}")]
        public async Task<IActionResult> DeletePassword(long id)
        {
            var ip = _context.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            var result = await _passwordService.DeletePwdAsync(_jwt.UserId,ip);
            return Ok(result);
        }

        /// <summary>
        /// 流量主，確認是否已經建立提現密碼
        /// </summary>
        /// <returns></returns>
        [HttpGet("checkexist")]
        public async Task<IActionResult> CheckExist()
        {
            var result = await _passwordService.CheckPasswordExist(_jwt.UserId);
            return Ok(result);
        }
        #endregion password

        #region bank


        /// <summary>
        /// 流量主--依帳戶取得收款訊息列表 ,下拉選單可用
        /// </summary>
        /// <returns></returns>
        [HttpGet("bank")]
        public async Task<IActionResult> GetBankByAccount(int page , int rows)
        {
            var result = await _bankService.GetBankByAccount(_jwt.UserId, page, rows);
            return Ok(result);
        }
        

        /// <summary>
        /// 流量主--新增收款信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("bank")]
        public async Task<IActionResult> CreateBank(CreateUpdateBankRequest request)
        {
            var accountid = _jwt.UserId;
            var result = await _bankService.CreateBankAsync(request, accountid);
            return Ok(result);
        }

        /// <summary>
        /// 流量主--修改收款訊息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("bank")]
        public async Task<IActionResult> UpdateBank(CreateUpdateBankRequest request)
        {
            var accountid = _jwt.UserId;
            var result = await _bankService.UpdateBankAsync(request, accountid);
            return Ok(result);
        }

        /// <summary>
        /// 流量主--刪除銀行帳號
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("bank/{id}")]
        public async Task<IActionResult> DeleteBank(long id)
        {
            var result = await _bankService.DeleteBankAsync(id, _jwt.UserId);
            return Ok(result);
        }
        #endregion

        
        #region record

        /// <summary>
        /// 管理後台 取得用戶待提現列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("record/all")]
        public async Task<IActionResult> GetRecordForMerchant(int page, int rows)
        {
            var result = await _recordService.GetAll(page, rows);
            return Ok(result);
        }

        /// <summary>
        /// 流量主-提現記錄
        /// </summary>
        /// <returns></returns>
        [HttpGet("record")]
        public async Task<IActionResult> MerchantGetRecord(int page , int rows)
        {
            var result = await _recordService.MerchantGetRecord(_jwt.UserId, page, rows);
            return Ok(result);
        }
        /// <summary>
        /// 依紀錄ID取得出金資料(水單)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("record/{id}")]
        public async Task<IActionResult> GetRecordById(long id)
        {
            var result = await _recordService.GetRecordById(id);
            return Ok(result);
        }
        /// <summary>
        /// 流量主-申請出金
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("record")]
        public async Task<IActionResult> CreateRecord(CreateRecordRequest request)
        {
            request.ip = _context.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            var accountid = _jwt.UserId;
            var result = await _recordService.ApplyRecordAsync(request,accountid);
            return Ok(result);
        }
        /// <summary>
        /// 公司後台-確認出金
        /// </summary>
        /// <param name="request">需要accountid ,token取得ID為執行付款者ID</param>
        /// <returns></returns>
        [HttpPatch("paid")]
        public async Task<IActionResult> Paid(PaidRequest request)
        {
            var payaccountid = _jwt.UserId;
            var result = await _recordService.PaidAsync(request, payaccountid);
            return Ok(result);
        }
        /// <summary>
        /// 流量主人工申請，我方客服-取消出金
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpDelete("record/{id}")]
        public async Task<IActionResult> Cancel(CancelRequest request)
        {
            var result = await _recordService.CancelAsync(request);
            return Ok(result);
        }
        /// <summary>
        /// 搜尋
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("query")]
        public async Task<IActionResult> Query([FromQuery]QueryRequest request)
        {
            var result = _recordService.Query(request);
            return Ok(result);
        }
       /// <summary>
       /// 改變通知狀態
       /// </summary>
       /// <param name="account_id"></param>
       /// <param name="record_id"></param>
       /// <param name="notify">0 新增紀錄1已通知管理員2 需要通知客戶(已付款)3 客戶已通知</param>
       /// <returns></returns>
        [HttpPut("notify")]
        public async Task<IActionResult> Notify(long account_id,long record_id,int notify)
        {
            var result = await _recordService.NotifyAsync(account_id,record_id,notify);
            return Ok(result);
        }

        #endregion record 
    }
}
