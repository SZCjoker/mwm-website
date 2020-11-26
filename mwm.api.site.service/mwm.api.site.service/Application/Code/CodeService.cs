using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MWM.API.Site.Service.Application.Code.Contract;
using MWM.API.Site.Service.Application.Common;
using MWM.API.Site.Service.Domain.Code;
using Phoenixnet.Extensions.Handler;
using Phoenixnet.Extensions.Object;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

namespace MWM.API.Site.Service.Application.Code
{
    public class CodeService : ICodeService
    {
        protected readonly ICodeRepository _repository;
        protected readonly IHttpContextAccessor _context;
        protected readonly IGenerateId _generate;        
        protected readonly IOptions<AppSettings> _settings;
        protected readonly ILogger<CodeService> _logger;

        public CodeService(IGenerateId generate, ICodeRepository repository, ILogger<CodeService> logger, IHttpContextAccessor context, IOptions<AppSettings> settings)
        {
            _repository = repository;
            _generate = generate;
            _context = context;
            _settings = settings;
            _logger = logger;
        }

        public async ValueTask<BasicResponse> CreateAsync(CreateUpdateCodeRequest request)
        {
            var date = Convert.ToInt32(DateTimeOffset.UtcNow.ToString("yyyyMMdd"));
            var time = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            var data = new CodeEntity()
            {   
                AccountId = request.account_id,
                CodeCnzz = request.code_cnzz,
                Code51la = request.code_51la,
                CodeGA = request.code_ga, 
                CodeAlly = request.code_ally,
                Cdate = date,
                Ctime = time,
                State = 1
            };

            var count = await _repository.Create(data);
            return StateCodeHandler.ForCount(count);
        }

        public async ValueTask<BasicResponse> UpdateAsync(CreateUpdateCodeRequest request)
        {
            var date = Convert.ToInt32(DateTimeOffset.UtcNow.ToString("yyyyMMdd"));
            var time = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            var data = new CodeEntity()
            {
                Id = request.id,
                AccountId = request.account_id,
                CodeCnzz = request.code_cnzz=="" ? string.Empty: request.code_cnzz,
                Code51la = request.code_51la == "" ? string.Empty : request.code_51la,
                CodeGA = request.code_ga == "" ? string.Empty : request.code_ga,
                CodeAlly = request.code_ally == "" ? string.Empty : request.code_ally,
                Udate = date,
                Utime = time,
                State = request.state
            };

            var count = await _repository.Update(data);
            return StateCodeHandler.ForCount(count);
        }

        public async ValueTask<BasicResponse> DeleteAsync(long accountId)
        {
            var count = await _repository.Delete(accountId);
            return StateCodeHandler.ForCount(count);
        }

        public async ValueTask<BasicResponse<CodeResponse>> GetCodeByAccount(long accountid)
        {
            var result = await _repository.ReadByAccount(accountid);
            var data = new CodeResponse
            {
                code_cnzz = result.CodeCnzz,
                code_51la = result.Code51la,
                code_ga = result.CodeGA,
                code_ally = result.CodeAlly

            };
            return new BasicResponse<CodeResponse> { code = 200, desc = "advert code", data = data }; 
        }

        public async ValueTask<PagingResponse<IEnumerable<CodeResponse>>> GetALL(int pageoffset, int pagesize)
        {

            var paging = new Paging(pageoffset,pagesize);

            var (rows ,total) = await _repository.ReadAll(paging.Offset,paging.PageSize);

            var data = rows.Select(d => new CodeResponse 
            { 
                id = d.Id,
                account_id = d.AccountId,
                code_51la = d.Code51la,
                code_cnzz = d.CodeCnzz,
                code_ga = d.CodeGA,
                code_ally = d.CodeAlly,
                cdate = d.Cdate,
                ctime = d.Ctime,
                udate = d.Udate,
                utime = d.Utime,
                state =d.State
            });

            return new PagingResponse<IEnumerable<CodeResponse>> {code=200,desc="success",data=data,paging=paging};
        }

        public async ValueTask<BasicResponse<CodeResponse>> GetCompanyCode(long managerid)
        {
            var result = await _repository.GetCompanyCode(managerid);
            return StateCodeHandler.ForBool<CodeResponse>((result.Id != 0), new CodeResponse()
            {
                account_id = result.AccountId,
                cdate = result.Cdate,
                ctime = result.Ctime,
                udate = result.Udate,
                utime = result.Utime,
                id = result.Id,
                state = result.State,
                code_cnzz = result.CodeCnzz,
                code_51la = result.Code51la,
                code_ga = result.CodeGA,
                code_ally = result.CodeAlly
            });
        }
    }
}