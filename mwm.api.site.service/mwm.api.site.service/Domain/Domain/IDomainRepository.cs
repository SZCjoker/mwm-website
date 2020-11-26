using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace MWM.API.Site.Service.Domain.Domain
{
    public  interface IDomainRepository
    {

        #region for website domain
        Task<int> CreateWebsiteDomain(WebSiteDomainEntity entity);
        Task<(IEnumerable<WebSiteDomainEntity> rows, long total)> GetWebsiteDomains(int pageoffset, int pagesize);
        Task<(IEnumerable<WebSiteDomainEntity>rows,long total)> GetWebsiteDomainById(long accountid ,int pageoffset, int pagesize);
        Task<int> UpdateWebsiteDomain(WebSiteDomainEntity entity);
        Task<int> DeleteWebsiteDomain(WebSiteDomainEntity entity);              
        Task<int> CheckWebsiteId(long id);
        Task<IEnumerable<WebSiteDomainEntity>> GetWebdomainSettingByAccount(long accountid);
        #endregion


        #region for dispatch domain
        Task<int> CheckDispatchId(long id);
        Task<long> GetDispatchDomainID(long accountid);
        Task<(IEnumerable<DispatchEntity>rows,long total)> GetDispatchDomain(int pageoffset,int pagesize);
        Task<DispatchEntity> GetDispatchDomainNameByAccount(long accountid);
        Task<int> CreateDispatchData(DispatchEntity entity);
        Task<int> CheckAccountId(long accountid);
        #endregion


        #region for video-domain use
        Task<int> CheckDomainId(long id);

        Task<int> CreateDomain(DomainEntity entity);

        Task<IEnumerable<DomainEntity>> GetDomains();

        Task<DomainEntity> GetDomainById(long id);

        Task<int> UpdateDomain(List<DomainEntity> entity);

        Task<int> DeleteDomain(long id);
        #endregion
    }
}
