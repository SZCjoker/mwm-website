using MWM.API.Advert.Service.Application.Common;
using MWM.API.Advert.Service.Application.Receivable.Contract;
using Phoenixnet.Extensions.Object;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MWM.API.Advert.Service.Application.Receivable
{
    public interface IReceivableBankService
    {
        ValueTask<PagingResponse<IEnumerable<BankResponse>>> GetBankByAccount(long accountId, int page, int rows);
       // ValueTask<BasicResponse<BankResponse>> GetBankById(long id, long accountId);
        ValueTask<BasicResponse> CreateBankAsync(CreateUpdateBankRequest request,long accountid);
        ValueTask<BasicResponse> UpdateBankAsync(CreateUpdateBankRequest request, long accountid);
        ValueTask<BasicResponse> DeleteBankAsync(long id, long accountId);
    } 
}
