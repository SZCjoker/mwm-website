using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MWM.API.Advert.Service.Application.Common;
using MWM.API.Advert.Service.Application.Common.CustomException;
using MWM.API.Advert.Service.Application.Receivable.Contract;
using MWM.API.Advert.Service.Domain.Receivable;
using Phoenixnet.Extensions.Handler;
using Phoenixnet.Extensions.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MWM.API.Advert.Service.Application.Receivable
{
    public class ReceivableBankService : IReceivableBankService
    {
        protected readonly IReceivableBankRepository _bankRepository; 
        protected readonly IGenerateId _generate;
        protected readonly IHttpContextAccessor _context;
        protected readonly IOptions<AppSettings> _settings;
        protected readonly ILogger<ReceivableBankService> _logger;


        public ReceivableBankService(IGenerateId generate, IReceivableBankRepository bankRepository, ILogger<ReceivableBankService> logger, IHttpContextAccessor context, IOptions<AppSettings> settings)
        {
            _bankRepository = bankRepository; 
            _generate = generate;
            _context = context;
            _settings = settings;
            _logger = logger;
        }

        #region bank

        public async ValueTask<BasicResponse> CreateBankAsync(CreateUpdateBankRequest request, long accountid)
        {

            if (accountid == 0) { throw new TokenException(); }
            request.account_id = accountid;
            var date = Convert.ToInt32(DateTimeOffset.UtcNow.ToString("yyyyMMdd"));
            var time = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            var data = new ReceivableBankEntity()
            {
                Id = _generate.GetId(),
                Address = request.address,
                AccountId = request.account_id,
                City = request.city,
                BankNumber = request.bank_number,
                Province = request.province,
                BankName = request.bank_name,
                UserName = request.user_name,
                Cdate=date,
                Ctime=time,
                State = 1
            };

            var count = await _bankRepository.Create(data);
            return StateCodeHandler.ForCount(count);
        }

        public async ValueTask<BasicResponse> UpdateBankAsync(CreateUpdateBankRequest request, long accountid)
        {
            if (accountid == 0) { throw new TokenException(); }
            request.account_id = accountid;
            var date = Convert.ToInt32(DateTimeOffset.UtcNow.ToString("yyyyMMdd"));
            var time = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            var data = new ReceivableBankEntity()
            {
                Id = request.id,
                Address = request.address,
                AccountId = request.account_id,
                City = request.city,
                BankNumber = request.bank_number,
                Province = request.province,
                BankName = request.bank_name,
                UserName = request.user_name,
                Udate = date,
                Utime =time,
            };

            var count = await _bankRepository.Update(data);
            return StateCodeHandler.ForCount(count);
        }

        public async ValueTask<BasicResponse> DeleteBankAsync(long id, long accountId)
        {
            if (accountId == 0) { throw new TokenException(); }
            var count = await _bankRepository.Delete(id, accountId, 99);
            return StateCodeHandler.ForCount(count);
        }

        public async ValueTask<PagingResponse<IEnumerable<BankResponse>>> GetBankByAccount(long accountId, int page, int rows)
        {
            if (accountId == 0) { throw new TokenException(); }
            var paging = new Paging(page, rows);
            var (entity, total) = await _bankRepository.ReadByAccountId(accountId,99, paging.Offset, rows);
             var datas =  entity.Select(d=>
                         new BankResponse()
                        {
                            account_id = d.AccountId,
                            address = d.Address,
                            city = d.City,
                            bank_number = d.BankNumber,
                            id = d.Id,
                            province = d.Province,
                            state = d.State,
                            bank_name = d.BankName,
                            user_name = d.UserName
                        });
            paging.RowsTotal = datas.Count();
            var coUnt= datas.Count();
            return StateCodeHandler.ForPagingCount(coUnt,datas,paging);
        }

        //public async ValueTask<BasicResponse<BankResponse>> GetBankById(long id, long accountId)
        //{
        //    var data = await _bankRepository.ReadById(id, accountId, 99);

        //    if (data == null)
        //        return new BasicResponse<BankResponse>() { code = 300, desc = "no_match_data", data = null };

        //    var model = new BankResponse()
        //    {
        //        account_id = data.AccountId,
        //        address = data.Address,
        //        city = data.City,
        //        bank_number = data.BankNumber,
        //        id = data.Id,
        //        province = data.Province,
        //        state = data.State,
        //        bank_name = data.BankName,
        //        user_name = data.UserName
        //    };

        //    return StateCodeHandler.ForBool<BankResponse>((data.Id != 0), model);
        //}

        #endregion 
    }
}
