using MWM.API.Advert.Service.Application.Receivable.Contract;
using MWM.API.Advert.Service.Domain.Receivable;
using Phoenixnet.Extensions.Object;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MWM.API.Advert.Service.Application.Receivable
{
    public interface IReceivablePasswordService
    {
        ValueTask<BasicResponse> ResetPassword(long account_id);
        ValueTask<BasicResponse<ReceivablePasswordEntity>> GetPwdByAccount(long accountid);
        ValueTask<BasicResponse<ReceivablePasswordEntity>> GetPwdById(long id, long accountid);
        ValueTask<BasicResponse> CreatePwdAsync(CreatePasswordRequest request,long accountid);
        ValueTask<BasicResponse> UpdatePwdAsync(UpdatePasswordRequest request, long accountid);
        ValueTask<BasicResponse> DeletePwdAsync(long accountid,string ip);
        ValueTask<PagingResponse<IEnumerable<PasswordResponse>>> GetAllPasswrod(int pageoffset,int pagesize);
        ValueTask<BasicResponse> CheckPasswordExist(long accountid);

    }
 
}
