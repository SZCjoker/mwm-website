using Phoenixnet.Extensions.Object;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MWM.API.Site.Service.Application.Domain
{
    public  interface IDomainService
    {
        #region for website
        Task<BasicResponse> CreateSiteDomain(WebSiteDomainRequest request,long accountid);
        Task<PagingResponse<IEnumerable<WebSiteDomainReponse>>> GetSiteDomains(int pageoffset,int pagesize);
        Task<PagingResponse<IEnumerable<WebSiteDomainReponse>>> GetSiteDomainById(long accountid ,int pageoffset, int pagesize);
        Task<BasicResponse> UpdateSiteDomain(WebSiteDomainRequest request);
        Task<BasicResponse> DeleteSiteDomain(long domainid);  
        Task<int> CheckWebsiteId(long id);
        Task<long> GetDispatchDomainID(long accountid);

        Task<BasicResponse<IEnumerable<WebSiteDomainReponse>>> GetWebdomainSettingByAccount(long accountid);

        #endregion

        #region for dispatch
        Task<int> CheckDispatchId(long id);
        Task<BasicResponse<int>> GetDispatchDomainId(long accountid);
        Task<BasicResponse<DispatchResponse>> GetDispatchDomainNameByAccount(long accountid);
        Task<PagingResponse<IEnumerable<DispatchResponse>>> GetDispatchDomain(int page ,int size);
        Task<BasicResponse> CreateDispatchData(DispatchRequest request);
        Task<int> CheckAccountId(long accountid);
        #endregion

        #region for domain use
        Task<int> CheckDomainId(long id);
        Task<BasicResponse> CreateDomain(VideoDomainRequest detail);

        Task<BasicResponse<IEnumerable<DomainResponse>>> GetDomains();

        Task<BasicResponse> UpdateDomain(List<VideoDomainRequest> request);

        Task<BasicResponse> DeleteDomain(long id);
        #endregion

       
        string GenDomain();
    }
}
