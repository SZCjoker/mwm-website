using MWM.API.Account.Service.Application.Common;
using MWM.API.Account.Service.Application.Bulletin.Contract;
using Phoenixnet.Extensions.Object;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MWM.API.Account.Service.Application.Bulletin
{
    public interface IBulletinService
    {
       
        ValueTask<PagingResponse<IEnumerable<BulletinResponse>>> GetAll(int page , int rows);

        
        ValueTask<BasicResponse<BulletinResponse>> GetById(long id);

        
        ValueTask<BasicResponse> CreateAsync(CreateUpdateBulletinRequest request,long accountid,string longiname);

       
        ValueTask<BasicResponse> UpdateAsync(CreateUpdateBulletinRequest request);

       
        ValueTask<BasicResponse> DeleteAsync(long id);

        ValueTask<PagingResponse<IEnumerable<BulletinResponse>>> Query(string title, int pageoffset, int page);
    }
}
