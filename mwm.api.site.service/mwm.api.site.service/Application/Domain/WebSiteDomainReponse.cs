using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MWM.API.Site.Service.Application.Domain
{
    public class WebSiteDomainReponse
    {
        public long ?id { get; set; }
        public long ?account_id { get; set; }
        public long ?dispatch_domain_id { get; set; }
        public long ?template_id { get; set; }
        public string public_domain { get; set; }
        public string website_name { get; set; }
        public string website_desc { get; set; }
        public int ?cdate{ get; set; }
        public Int64 ?ctime { get; set; }
        public int ?udate { get; set; }
        public Int64 ?utime { get; set; }
        public string keyword { get; set; }
        public int ?state { get; set; }        
    }
}
