using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MWM.API.Site.Service.Domain.Site;
using Phoenixnet.Extensions.Object;

namespace MWM.API.Site.Service.Domain
{
 public interface ISiteRepository
    {
        Task<int> InitializeWebsite(SiteInfoEntity entity);
        Task<int> UpdateWebsiteInfo(SiteInfoEntity entity);
        Task<int> DeleteWebsite(SiteInfoEntity entity);
        Task<(IEnumerable<SiteInfoEntity> rows,long total)> GetWebsiteDetails(int pageoffset, int pagesize, string queryStr);
        Task<SiteInfoEntity> GetWebsiteDetailsById(long accountid);
        Task<int> CheckId(long id);
        ValueTask<SiteInfoEntity> GetCompanySiteData(long managerid);
    }
}
