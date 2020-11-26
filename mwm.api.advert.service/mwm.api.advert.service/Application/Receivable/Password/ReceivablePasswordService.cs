using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MWM.API.Advert.Service.Application.Common;
using MWM.API.Advert.Service.Application.Receivable.Contract;
using MWM.API.Advert.Service.Domain.Receivable;
using Phoenixnet.Extensions.Handler;
using Phoenixnet.Extensions.Object;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Threading.Tasks;
using MWM.API.Advert.Service.Application.Common.CustomException;

namespace MWM.API.Advert.Service.Application.Receivable
{
    public class ReceivablePasswordService : IReceivablePasswordService
    { 
        protected readonly IReceivablePasswordRepository _passwordRepository; 
        protected readonly IGenerateId _generate;
        protected readonly IHttpContextAccessor _context;
        protected readonly IOptions<AppSettings> _settings;
        protected readonly ILogger<ReceivablePasswordService> _logger;


        public ReceivablePasswordService(IGenerateId generate, IReceivablePasswordRepository configRepository, ILogger<ReceivablePasswordService> logger, IHttpContextAccessor context, IOptions<AppSettings> settings)
        { 
            _passwordRepository = configRepository; 
            _generate = generate;
            _context = context;
            _settings = settings;
            _logger = logger;
        } 

        public async ValueTask<BasicResponse> CreatePwdAsync(CreatePasswordRequest request, long accountid)
        {
            int count = 0;
            if (accountid == 0) { throw new TokenException(); };
            request.account_id = accountid;
            var date = Convert.ToInt32(DateTimeOffset.UtcNow.ToString("yyyyMMdd"));
            var time = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            var check = await GetPwdByAccount(request.account_id);
            if(check.data.AccountId != 0 )
            {
                var ResetRequest = new ReceivablePasswordEntity
                {
                    Id = check.data.Id,
                    AccountId = check.data.AccountId,
                    Password = request.password,
                    FailCount = 0,
                    LoginIp = request.ip,
                    Udate = date,
                    Utime = time,
                    State = 1
                };
                 count = await _passwordRepository.Create(ResetRequest);
                return StateCodeHandler.ForCount(count);
            }
            var data = new ReceivablePasswordEntity()
            {
                Id = _generate.GetId(),
                AccountId = request.account_id,
                Password = request.password,
                Cdate = date,
                Ctime = time,
                FailCount=0,
                LoginIp=request.ip,
                Udate=date,
                Utime=time,
                State = 1
            };
             count = await _passwordRepository.Create(data);
            return StateCodeHandler.ForCount(count);
        } 

        public async ValueTask<BasicResponse> UpdatePwdAsync(UpdatePasswordRequest request, long accountid)
        {
            if (accountid == 0) { throw new TokenException(); };
            request.account_id = accountid;
            var date = Convert.ToInt32(DateTimeOffset.UtcNow.ToString("yyyyMMdd"));
            var time = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var old = await _passwordRepository.ReadByAccountId(request.account_id,99);

            if (old.Password != request.password)
            {
                if (old.FailCount == 3)
                {
                    await _passwordRepository.Delete(request.account_id, 99, request.ip);
                    return new BasicResponse { code = 301, desc = "舊密碼比對超過三次已鎖定，請洽客服人員" };
                }

                var faildata = new ReceivablePasswordEntity()
                {
                    Id = request.id,
                    AccountId = request.account_id,
                    Password = "",
                    LoginIp = request.ip,
                    FailCount = Convert.ToInt16(old.FailCount + 1),
                    Udate = date,
                    Utime = time,
                    State = request.state
                };
                await _passwordRepository.Update(faildata);
                return new BasicResponse { code = 301, desc = "與舊密碼比對不相符" };
            }
            
            var data = new ReceivablePasswordEntity()
            {
                Id = request.id,
                AccountId = request.account_id,
                Password = request.newpassword,
                LoginIp = request.ip,
                Udate=date,
                Utime=time,
                State = request.state
            };
            var result = await _passwordRepository.Update(data);
            return StateCodeHandler.ForCount(result);
        }

        public async ValueTask<BasicResponse> DeletePwdAsync(long accountid,string ip)
        {
            if (accountid == 0) { throw new TokenException(); }
            var count = await _passwordRepository.Delete(accountid, 99,ip);
            return StateCodeHandler.ForCount(count);
        }

        public async ValueTask<BasicResponse<ReceivablePasswordEntity>> GetPwdByAccount(long accountid)
        {
            if (accountid == 0) { throw new TokenException(); }
            var data = await _passwordRepository.ReadByAccountId(accountid, 99);
            return StateCodeHandler.ForNull<ReceivablePasswordEntity>(data);
        }

        public async ValueTask<BasicResponse<ReceivablePasswordEntity>> GetPwdById(long id, long accountid)
        {
            if (accountid == 0) { throw new TokenException(); }

            var data = await _passwordRepository.ReadById(id, accountid, 99);

            if (data == null)
                return new BasicResponse<ReceivablePasswordEntity>() { code = 300, desc = "no_match_data", data = null };

            return StateCodeHandler.ForBool<ReceivablePasswordEntity>((data.Id != 0), data);
        }

        public async ValueTask<PagingResponse<IEnumerable<PasswordResponse>>> GetAllPasswrod(int pageoffset,int pagesize)
        {
            var paging = new Paging(pageoffset,pagesize);

            var (rows,total) = await _passwordRepository.ReadAll(99, paging.Offset, paging.PageSize);

            var data = rows.Select(d => new PasswordResponse
            {
                id = d.Id,
                account_id = d.AccountId,
                password = d.Password,
                login_ip = d.LoginIp,
                fail_count = d.FailCount,
                udate = d.Udate,
                utime = d.Utime,
                state = d.State
            });

            return new PagingResponse<IEnumerable<PasswordResponse>> { code=200,desc="success",data = data,paging= paging};

        }

        public async  ValueTask<BasicResponse> CheckPasswordExist(long accountid)
        {
            if (accountid == 0) { throw new TokenException(); }
                var data = await _passwordRepository.Check(accountid);
                return StateCodeHandler.ForCount(data);
        }

        public async ValueTask<BasicResponse> ResetPassword(long account_id)
        {
            Guid G = Guid.NewGuid();
            var nPwd = G.ToString();

            if (account_id == 0) { throw new TokenException($"need customer ID"); }
            var result = await _passwordRepository.ResetPassword(account_id,nPwd);
            return new BasicResponse { code = 200, desc = $"new password:{nPwd}" };
        }
    }
}