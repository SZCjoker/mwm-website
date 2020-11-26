using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MWM.API.Site.Service.Domain.Domain
{
    public class WebSiteDomainEntity
    {
        public long Id { get; set; }
        public long AccountId { get; set; }
        public long DispatchDomainId { get; set; }
        public long TemplateId { get; set; }
        public string PubDomain { get; set; }
        public string WebsiteName { get; set; }
        public string WebsiteDesc { get; set; }
        public string Keyword { get; set; }
        public  int Cdate { get; set; }
        public Int64 Ctime { get; set; }
        public int Udate { get; set; }
        public Int64 Utime { get; set; }
        public int State { get; set; }
    }
}
