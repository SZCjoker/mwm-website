using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MWM.API.Site.Service.Application.Site.Datatype;
using Phoenixnet.Extensions.Object;

namespace MWM.API.Site.Service.Application.Site
{
 public interface ISiteService
    {
        Task<BasicResponse> CreateWebsite(WebDetailRequest request, long accountid);
        Task<BasicResponse> UpdateWebsite(WebDetailRequest request);
        Task<BasicResponse> DeleteWebsite(int accountid);
        Task<PagingResponse<IEnumerable<WebDetailResponse>>> GetWebsite(int pageoffset, int pagesize);
        Task<BasicResponse<WebDetailResponse>> GetWebsiteById(int accountid);
        ValueTask<BasicResponse<WebDetailResponse>> GetCompanySiteData(long managerid);
    }
}
