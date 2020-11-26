using MWM.API.Site.Service.Application.Code.Contract;
using Phoenixnet.Extensions.Object;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace MWM.API.Site.Service.Application.Code
{
    public interface ICodeService
    {
        ValueTask<PagingResponse<IEnumerable<CodeResponse>>> GetALL(int pageoffset,int pagesize);
        ValueTask<BasicResponse<CodeResponse>> GetCodeByAccount(long accountid);
        ValueTask<BasicResponse<CodeResponse>> GetCompanyCode(long managerid);
        ValueTask<BasicResponse> CreateAsync(CreateUpdateCodeRequest request);
        ValueTask<BasicResponse> UpdateAsync(CreateUpdateCodeRequest request);
        ValueTask<BasicResponse> DeleteAsync(long accountId);
    }
}
