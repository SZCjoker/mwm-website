using Phoenixnet.Extensions.Object;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MWM.API.Site.Service.Application.Link
{
    public interface ILinkService
    {
        ValueTask<BasicResponse<LinkResponse>> GetById(long id);

        ValueTask<BasicResponse> CreateAsync(CreateUpdateLinkRequest request);

        ValueTask<BasicResponse> UpdateAsync(CreateUpdateLinkRequest request);

        ValueTask<BasicResponse> DeleteAsync(long id, long accountId);

        ValueTask<BasicResponse<IEnumerable<LinkResponse>>> GetAll(long accountid); 
    }
}
