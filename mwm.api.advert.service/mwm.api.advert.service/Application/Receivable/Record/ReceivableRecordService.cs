using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MWM.API.Advert.Service.Application.Common;
using MWM.API.Advert.Service.Application.Common.CustomException;
using MWM.API.Advert.Service.Application.Receivable.Contract;
using MWM.API.Advert.Service.Application.Receivable.Record;
using MWM.API.Advert.Service.Domain.Receivable;
using MWM.API.Advert.Service.Domain.Receivable.Record;
using Phoenixnet.Extensions.Handler;
using Phoenixnet.Extensions.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MWM.API.Advert.Service.Application.Receivable
{
    public class ReceivableRecordService : IReceivableRecordService
    {
        protected readonly IReceivableRecordRepository _recordRepository;
        protected readonly IReceivablePasswordRepository _passrepository;
        protected readonly IReceivablePasswordService _passwordservice;
        protected readonly IReceivableBankService _bankservice;
        protected readonly IGenerateId _generate;
        protected readonly IHttpContextAccessor _context;
        protected readonly IOptions<AppSettings> _settings;
        protected readonly ILogger<ReceivableRecordService> _logger;

        public ReceivableRecordService(IGenerateId generate, 
                                       IReceivableRecordRepository recordRepository, 
                                       ILogger<ReceivableRecordService> logger, 
                                       IHttpContextAccessor context, 
                                       IOptions<AppSettings> settings,
                                       IReceivablePasswordService passwordService,
                                       IReceivablePasswordRepository passwordRepository,
                                       IReceivableBankService bankService)
        {
            _recordRepository = recordRepository;
            _passwordservice = passwordService;
            _passrepository = passwordRepository;
            _bankservice = bankService;
            _generate = generate;
            _context = context;
            _settings = settings;
            _logger = logger;
        }

        public async ValueTask<BasicResponse> ApplyRecordAsync(CreateRecordRequest request, long accountid)
        {
            if (accountid == 0) { throw new TokenException(); }
            request.account_id = accountid;

            //出金後 金額要先扣掉
            var date = Convert.ToInt32(DateTimeOffset.UtcNow.ToString("yyyyMMdd"));
            var time = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            //檢查密碼
            var passChk = await _passwordservice.GetPwdByAccount(request.account_id);
            //檢查有無收款信息
            var bankChk = await _bankservice.GetBankByAccount(request.account_id,0,10);
            // 當日是否己有提現
            var exist = await _recordRepository.CheckByDate(request.account_id, 1, date);
            if ( passChk.data.Password != request.password)
            {
                

                if (passChk.data.FailCount == 3)
                {
                     await  _passrepository.Delete(request.account_id, 99, request.ip);
                    return new BasicResponse { code = 301, desc = "密碼錯誤超過三次已鎖定，請洽客服人員" };
                }
                var faildata = new ReceivablePasswordEntity { AccountId = request.account_id, 
                                                              FailCount = Convert.ToInt16(passChk.data.FailCount+1),
                                                              Password="",LoginIp = request.ip };
                await _passrepository.Update(faildata);
                
                return new BasicResponse { code=301 ,desc="密碼錯誤"};
            }
            
            if (!bankChk.data.Any(b => b.account_id == request.account_id))
            {
              
                return new BasicResponse { code = 301, desc = "請先新增收款信息" };
            }

            if (exist == 1)
            {
                return new BasicResponse() { code = 301, desc = "今日己有提現記錄" };
            }

            if(request.pay_amount < 55)//金額限制部分待修改
            {
                if(request.pay_amount > 100)
                {
                    return new BasicResponse() { code = 301, desc = "超過最大一次提領金額" };
                }
                return new BasicResponse() { code = 301, desc = "金額不足" };
            }

            var data = new RecordEntity()
            {
                Id = _generate.GetId(),
                AccountId = request.account_id,
                BankId = request.bank_id,
                PayAmount =request.pay_amount,                
                Cdate = date,
                Ctime = time,
                IsNotify = 0,   
                IsPaid = 0,
            };

            var count = await _recordRepository.Create(data);
            return StateCodeHandler.ForCount(count);
        }
        //already pay
        public async ValueTask<BasicResponse> PaidAsync(PaidRequest request,long accountid)
        {
            if (accountid == 0) { throw new TokenException($"need login again"); }
            request.pay_account_id = accountid;
            var date = Convert.ToInt32(DateTimeOffset.UtcNow.ToString("yyyyMMdd"));
            var time = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            var data = new RecordEntity()
            {
                Id = request.id,
                AccountId = request.account_id,                
                PayAccountId = request.pay_account_id,
                PayAmount = request.pay_amount,
                PayDate =date,
                PayTime = time,
                IsPaid = 1
            };
            var count = await _recordRepository.Paid(data);
            //notify
            if(count!=0)
            { await _recordRepository.Notify(data.Id,data.AccountId,2); }
            return StateCodeHandler.ForCount(count);
        }

        public async ValueTask<BasicResponse> CancelAsync(CancelRequest request)
        {  //要把金額加回去
            var date = Convert.ToInt32(DateTimeOffset.UtcNow.ToString("yyyyMMdd"));
            var time = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            var data = new CancelEntity 
            { 
             Id = request.id,
             AccountId = request.account_id,
             IsPaid = request.is_paid,
             IsCancel = request.is_cancel,
             CancelReason =request.cancel_reason,
             Udate = date,
             Utime = time
            };
            var count = await _recordRepository.Cancel(data);
            return StateCodeHandler.ForCount(count);
        }

        public async ValueTask<PagingResponse<IEnumerable<CashOutRecordAccountResponse>>> ReadByAccountId(long accountid, int page, int rows)
        {
            var paging = new Paging(page, rows);
            var (entity,total) = await _recordRepository.ReadByAccountId(accountid,paging.Offset,paging.PageSize);
            var datas = entity.Select(d => new CashOutRecordAccountResponse 
            { 
             id = d.Id,
             account_id = d.AccountId,
             account_name = d.AccountName,
             bank_name = d.BankName,
             bank_number = d.BankNumber,
             user_name = d.UserName,
             apply_time = $"{d.Cdate},{d.Ctime}",
             pay_amount = d.PayAmount,
             pay_time = $"{d.PayDate},{d.PayTime}",
             is_notify = d.IsNotify,
             is_paid = d.IsPaid
            });

            paging.RowsTotal = datas.Count();
            var coUnt = datas.Count();
            return StateCodeHandler.ForPagingCount(coUnt,datas,paging);
        }

        public async ValueTask<BasicResponse<RecordResponse>> GetRecordById(long id)
        {
            var result = await _recordRepository.ReadById(id);

            var data = new RecordResponse()
            {
                account_id = result.AccountId,               
                id = result.Id,
                is_notify = result.IsNotify,                
                pay_account_id = result.PayAccountId,
                pay_amount = result.PayAmount,
                pay_time =$"{result.PayDate},{result.PayTime}",
                is_paid = result.IsPaid,                
            };

            return new BasicResponse<RecordResponse> { code = 200, data = data, desc = "success" };
        }

        public async ValueTask<PagingResponse<IEnumerable<RecordResponse>>> Query(QueryRequest request)
        {

            if (request.page_size == 0)
            {
                return new PagingResponse<IEnumerable<RecordResponse>> { code = 200, desc = $"請輸入分頁參數" };
            }
            var paging = new Paging(request.page_offset, request.page_size);
            var queryStr = string.Empty;

            if (!string.IsNullOrEmpty(request.login_name)) queryStr += $" AND a.login_name LIKE '%{request.login_name}%'";

            if (!string.IsNullOrEmpty(request.begin_date)) queryStr += $" AND from_unixtime(r.pay_time,'%Y%m%d') >= STR_TO_DATE({request.begin_date},'%Y%m%d')";

            if (!string.IsNullOrEmpty(request.end_date)) queryStr += $" AND from_unixtime(r.pay_time,'%Y%m%d') <= STR_TO_DATE({request.end_date},'%Y%m%d')";

            var queryData = new QueryEntity
            { 
              LoginName = request.login_name,
              PayDate = request.begin_date,
              PayTime = request.end_date,
              Fees = request.fees,
              IsPaid = request.is_paid,
              PageOffset = request.page_offset,
              PageSize = request.page_size
            };

            var (rows, total) = await _recordRepository.Query(queryData, queryStr);

            var datas = rows.Select(d => new RecordResponse
            { 
                id = d.Id,
                account_name = d.AccountName,
                bank_number = d.BankNumber,
                bank_name = d.BankName,
                user_name = d.UserName,
                bank_address = d.BankAddress,
                pay_time = $"{d.PayDate},{d.PayTime}",
                pay_amount = d.PayAmount,
                pay_account_id = d.PayAccountId
            });
            paging.RowsTotal = datas.Count();
            var coUnt = datas.Count();

            return StateCodeHandler.ForPagingCount<IEnumerable<RecordResponse>>(coUnt, datas, paging);
        }

        public async  ValueTask<PagingResponse<IEnumerable<CashOutRecordAccountResponse>>> GetAll(int page,int rows)
        {
            var paging = new Paging(page, rows);

            var (entity,total) = await _recordRepository.ReadAll(paging.Offset,paging.PageSize);

            var datas = entity.Select(d => new CashOutRecordAccountResponse
            {  
               account_id = d.AccountId,
               account_name =  d.AccountName,
               bank_name = d.BankName,
               bank_number = d.BankNumber,
               bank_address = d.BankAddress,
               user_name = d.UserName,
               apply_time = $"{d.Cdate},{d.Ctime}",
               pay_time = $"{d.PayDate},{d.PayTime}",
               pay_amount = d.PayAmount,
               is_paid = d.IsPaid
            });
            paging.RowsTotal = datas.Count();
            var coUnt = datas.Count();
            return StateCodeHandler.ForPagingCount(coUnt,datas,paging);
        }

        public async  ValueTask<PagingResponse<IEnumerable<RecordResponse>>> MerchantGetRecord(long accountid, int page, int rows)
        {
            if (accountid == 0) { throw new TokenException(); }
            var paging = new Paging(page, rows);
            var (entity, total) = await _recordRepository.MerchantGetRecord(accountid, paging.Offset, rows);

            var datas = entity.Select(d => new RecordResponse
            { 
                id = d.Id,
                account_id = d.AccountId,
                account_name = d.AccountName,
                bank_name = d.BankName,
                bank_number = d.BankNumber,
                user_name = d.UserName,
                pay_account_id = d.PayAccountId,
                pay_amount = d.PayAmount,
                apply_time = $"{d.Cdate},{d.Ctime}",
                pay_time = $"{d.PayDate},{d.PayTime}",
                is_notify = d.IsNotify,
                is_paid = d.IsPaid
            });
            paging.RowsTotal = datas.Count();
            var coUnt = datas.Count();
            return StateCodeHandler.ForPagingCount(coUnt,datas,paging);
        }

        public async ValueTask<BasicResponse> NotifyAsync(long accountid, long recordid,int notify)
        {
            var result = await _recordRepository.Notify(accountid, recordid,notify);
            return StateCodeHandler.ForCount(result);
        }
    }
}